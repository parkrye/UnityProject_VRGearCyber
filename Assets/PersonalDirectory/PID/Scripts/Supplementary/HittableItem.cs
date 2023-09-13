using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PID; 

namespace PID
{
    public class HittableItem : MonoBehaviour, IHitable, IStrikable
    {
        public enum ItemType
        {
            Head, 
            Ribcage
        }
        [SerializeField] int durability;
        Rigidbody rb;
        MeshRenderer meshRenderer;
        [SerializeField] Color highlightColor; 
        [SerializeField] float flightForce;
        [SerializeField] float highlightIntensity;
        [SerializeField] float highlightDuration;
        float highlightTimer;
        bool isWearing;
        public bool IsWearing
        {
            get => isWearing;
        }
        LayerMask usedState;
        #region WEAR INFO 
        Quaternion wearingRotation; 

        #endregion
        private void Awake()
        {
            isWearing = true; 
            rb = GetComponent<Rigidbody>();
            meshRenderer = GetComponent<MeshRenderer>();
            usedState = LayerMask.GetMask("WearableUsed"); 
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            durability--;
            StopAllCoroutine(); 
            StartCoroutine(HitEffect()); 
            if (durability <= 0)
            {
                isWearing = false; 
                rb.isKinematic = false;
                transform.parent = null;
                rb.AddForceAtPosition(hitNormal * flightForce, hitPoint, ForceMode.Impulse);
            }
        }
        public void TakeStrike(Transform hitter, float damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            if (durability == 0)
                return; 
            StopAllCoroutine(); 
            StartCoroutine(HitEffect());
            durability = 0;
            isWearing = false;
            rb.isKinematic = false;
            transform.parent = null;
            rb.AddForceAtPosition(hitNormal * flightForce, hitPoint, ForceMode.Impulse);
        }
        public void Deprecated()
        {
            gameObject.layer = usedState;
        }

        public void Retrieved(RobotParts bodyPart)
        {

        }

        Color prevColor;
        float lerpRatio;
        float intensity; 
        IEnumerator HitEffect()
        {
            highlightTimer = highlightDuration; 
            while(highlightTimer > 0)
            {
                highlightTimer -= Time.deltaTime;
                lerpRatio = Mathf.Clamp01(highlightTimer / highlightDuration); 
                intensity = highlightIntensity * lerpRatio;
                meshRenderer.material.color = highlightColor * intensity; 
                yield return null;
            }
        }
    }

}

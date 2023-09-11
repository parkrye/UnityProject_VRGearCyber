using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PID; 

namespace PID
{
    public class HittableItem : MonoBehaviour, IHitable, IStrikable
    {
        [SerializeField] int durability;
        Rigidbody rb;
        SkinnedMeshRenderer meshRenderer;
        [SerializeField] Color highlightColor; 
        [SerializeField] float flightForce;

        [SerializeField] float highlightIntensity;
        [SerializeField] float highlightDuration;
        float highlightTimer; 
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            meshRenderer = GetComponent<SkinnedMeshRenderer>();
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            durability--;
            if (durability <= 0)
            {
                rb.isKinematic = false;
                transform.parent = null;
                rb.AddForceAtPosition(hitNormal * flightForce, hitPoint, ForceMode.Impulse);
            }
        }
        public void TakeStrike(Transform hitter, float damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            durability = 0;
        }
        public void ReturnToAshes()
        {
            GameManager.Pool.Release(this);
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

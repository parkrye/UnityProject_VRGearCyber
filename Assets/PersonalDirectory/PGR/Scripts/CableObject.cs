using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGR
{
    public class CableObject : MonoBehaviour
    {
        [SerializeField] Vector3 fixTranPosotion;
        [SerializeField] Rigidbody rb;
        [SerializeField] bool isFixed;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        void LateUpdate()
        {
            if (!isFixed)
                return;

            transform.position = fixTranPosotion;
        }

        void FixEnter()
        {
            isFixed = true;
        }

        void FixExit()
        {
            isFixed = false;
        }

        public void ShotCable(Vector3 shotDirection, float shotPower)
        {
            FixExit();
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            rb.AddForce(shotDirection * shotPower, ForceMode.Impulse);
        }

        void OnCollisionEnter(Collision collision)
        {
            IHackable hackable = collision.gameObject.GetComponent<IHackable>();
            if (hackable == null)
                return;

            fixTranPosotion = transform.position;
            FixEnter();
        }

        void OnCollisionExit(Collision collision)
        {
            IHackable hackable = collision.gameObject.GetComponent<IHackable>();
            if (hackable == null)
                return;

            fixTranPosotion = Vector3.zero;
            FixExit();
        }
    }
}

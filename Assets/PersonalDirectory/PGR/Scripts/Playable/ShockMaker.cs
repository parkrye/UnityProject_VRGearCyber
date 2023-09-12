using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGR
{
    public class ShockMaker : MonoBehaviour
    {
        [SerializeField] List<Rigidbody> targets;
        [SerializeField] float power;

        void OnCollisionEnter(Collision collision)
        {
            Rigidbody target = collision.gameObject.GetComponent<Rigidbody>();
            if (target != null && targets.Contains(target))
            {
                target.AddForce(-collision.GetContact(0).normal * power, ForceMode.Impulse);
            }
        }
    }
}

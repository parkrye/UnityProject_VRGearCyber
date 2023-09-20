using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGR
{

    public class DoorOpening : MonoBehaviour
    {
        [SerializeField] int onStep;
        [SerializeField] Animator animator;
        
        void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                onStep++;
                animator.SetBool("Open", onStep > 0);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                onStep--;
                animator.SetBool("Open", onStep > 0);
            }
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    public class Censer : MonoBehaviour
    {
        public Elevator elevator;

        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(Close());
        }

        private void OnTriggerExit(Collider other)
        {
            StartCoroutine(Close());
        }

        IEnumerator Close()
        {
            yield return new WaitForSeconds(2f);
            elevator.Close();
        }
    }
}

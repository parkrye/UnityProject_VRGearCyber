using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PM
{
    public class material : MonoBehaviour
    {
        Material mat;
        public Material[] hackingMat;

        void Start()
        {
            StartCoroutine(start());
        }

        IEnumerator start()
        {
            HackingStart();
            yield return new WaitForSeconds(3f);
            HackingStop();
        }
        public void HackingStart()
        {
            transform.GetComponent<MeshRenderer>().material = hackingMat[1];
            Debug.Log(transform.GetComponent<MeshRenderer>().materials[0]);
        }
        public void HackingStop()
        {
            transform.GetComponent<MeshRenderer>().material = hackingMat[0];
            Debug.Log(transform.GetComponent<MeshRenderer>().materials[0]);
        }
    }

}

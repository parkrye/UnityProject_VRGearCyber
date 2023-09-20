using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PM
{
    public class MaterialChange : MonoBehaviour
    {
        Material mat;
        public Material[] hackingMat;

        public void HackingStart()
        {
            transform.GetComponent<MeshRenderer>().material = hackingMat[1];
        }
        public void HackingStop()
        {
            transform.GetComponent<MeshRenderer>().material = hackingMat[0];
        }
    }

}

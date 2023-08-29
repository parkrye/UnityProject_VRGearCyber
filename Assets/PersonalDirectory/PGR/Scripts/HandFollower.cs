using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGR
{
    public class HandFollower : MonoBehaviour
    {
        [SerializeField] Transform originTransform;

        void LateUpdate()
        {
            transform.position = originTransform.position;
            transform.rotation = originTransform.rotation;
        }
    }

}
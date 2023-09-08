using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGR
{
    public class SelfRotater : MonoBehaviour
    {
        enum Axis { X, Y, Z}
        [SerializeField] Axis axis;

        void FixedUpdate()
        {
            switch(axis)
            {
                case Axis.X:
                    transform.Rotate(Vector3.right,Time.fixedDeltaTime);
                    break;
                case Axis.Y:
                    transform.Rotate(Vector3.up, Time.fixedDeltaTime);
                    break;
                case Axis.Z:
                    transform.Rotate(Vector3.forward, Time.fixedDeltaTime);
                    break;
            }
        }
    }
}

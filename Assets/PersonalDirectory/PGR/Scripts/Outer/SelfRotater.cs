using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGR
{
    public class SelfRotater : MonoBehaviour
    {
        enum Axis { X, Y, Z}
        [SerializeField] Axis axis;
        [SerializeField] float speed;

        void FixedUpdate()
        {
            switch(axis)
            {
                case Axis.X:
                    transform.Rotate(Vector3.right, Time.fixedDeltaTime * speed);
                    break;
                case Axis.Y:
                    transform.Rotate(Vector3.up, Time.fixedDeltaTime * speed);
                    break;
                case Axis.Z:
                    transform.Rotate(Vector3.forward, Time.fixedDeltaTime * speed);
                    break;
            }
        }
    }
}

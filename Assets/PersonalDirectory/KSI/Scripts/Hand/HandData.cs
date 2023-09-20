using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSI
{
    public class HandData : MonoBehaviour
    {
        public enum HandModeType { Left, Right }

        public HandModeType handType;
        public Transform root;
        public Animator animator;
        public Transform[] fingerBones;
    }
}

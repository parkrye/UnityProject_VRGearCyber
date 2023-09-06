using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PID
{
    public interface IHearable
    {
        public void Heard(Vector3 soundPoint);
    }
}
using PID;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PID
{
    #region UNDER CONSTRUCTION
    public class AuditoryFunction : MonoBehaviour, IHearable
    {
        public UnityAction<Vector3> trackSound;
        public void Heard(Vector3 soundPoint, bool hasWall)
        {
            if (hasWall)
            {
                //Check for the valid distance 
            }
            else
            {
                trackSound?.Invoke(soundPoint);
            }
        }
    }
    //UNDER CONSTRUCTION
    #endregion
}
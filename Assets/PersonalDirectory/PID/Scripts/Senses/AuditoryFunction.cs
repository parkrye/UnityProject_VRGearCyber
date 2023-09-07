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
        public UnityAction<Vector3> SoundHeard;
        public UnityAction<Vector3> trackSound;
        public void Heard(Vector3 soundPoint)
        {
            if (WallIntersect(soundPoint))
            {
                Debug.Log("Wall Checked");
                return;
                //Check for the valid distance 
            }
            else
            {
                trackSound?.Invoke(soundPoint);
            }
        }

        RaycastHit hitInfo;
        float sqrDist;
        private bool WallIntersect(Vector3 destination)
        {
            Vector3 dir = destination - transform.position;
            sqrDist = dir.sqrMagnitude;
            if (Physics.Raycast(transform.position, dir.normalized, out hitInfo, LayerMask.GetMask("Wall")))
            {
                if (hitInfo.distance * 2 > sqrDist)
                    return false;
                return true;
            }
            Debug.Log($"Something is Wrong with the map, {destination} => {transform.position}");
            return false;
        }
    }
    //UNDER CONSTRUCTION
    #endregion
}
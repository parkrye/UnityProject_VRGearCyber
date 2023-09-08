using PID;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PID.RobotHelper; 
using UnityEngine.Events;

namespace PID
{
    #region UNDER CONSTRUCTION
    public class AuditoryFunction : MonoBehaviour, IHearable
    {
        bool UnderRadioSound;
        BaseEnemy enemyBody; 
        public UnityAction<Vector3> SoundHeard;
        public UnityAction<Vector3> trackSound;
        private void Awake()
        {
            enemyBody = GetComponent<BaseEnemy>();
            UnderRadioSound = false; 
        }
        Vector3 soundPointOnMesh; 
        public void Heard(Vector3 soundPoint, bool hasWall)
        {
            if (UnderRadioSound)
                return;

            ValidSoundCheckSlip newSoundSlip = ValidSoundPoint(soundPoint);
            if (!newSoundSlip.isValid)
                return;
            soundPointOnMesh = newSoundSlip.soundPointOnPath; 
            if (hasWall)
            {
                GameManager.traceSound.RequestPath(enemyBody.Agent, soundPoint, SetPath); 
                //Check for the valid distance 
            }
            else
            {
                trackSound?.Invoke(soundPointOnMesh);
            }
        }
        public void UnderRadio(bool underRadioSound)
        {
            UnderRadioSound = underRadioSound;
        }
        public void SetPath(Vector3 destination, bool success)
        {
            if (!success)
                return;
            trackSound?.Invoke(destination);
        }
    }
    //UNDER CONSTRUCTION
    #endregion
}
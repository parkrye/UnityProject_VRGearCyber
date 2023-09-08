using PID;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PID.SoundTraceHelper; 
using UnityEngine.EventSystems;

namespace PID
{
    public class DebugSoundTester : MonoBehaviour, IPointerClickHandler
    {
        Vector3 soundPoint;
        bool hasSound = false; 
        [SerializeField] float soundIntensity;
        public void OnPointerClick(PointerEventData eventData)
        {
            TriggerSound(eventData.pointerPressRaycast.worldPosition); 
        }

        public void TriggerSound(Vector3 soundPoint)
        {
            this.soundPoint = soundPoint;
            hasSound = true; 
            Collider[] colliders = Physics.OverlapSphere(soundPoint, soundIntensity);
            foreach (Collider collider in colliders)
            {
                IHearable hearer = collider.GetComponent<IHearable>();
                hearer?.Heard(transform.position, GameManager.traceSound.WallIntersect(soundPoint, collider.transform.position));
            }
        }

        private void OnDrawGizmos()
        {
            if (!hasSound)
                return; 
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(soundPoint, soundIntensity);
        }
    }
}

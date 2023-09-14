using PID;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PID
{
    public class SoundMaker : MonoBehaviour
    {
        [SerializeField] float soundIntensity;
        [SerializeField] float alarmInterval;
        Animator anim; 
        WaitForSeconds alarmWaitForSeconds;
        bool finishedAlarm; 
        public bool FinishedAlarm
        {
            get => finishedAlarm; 
            set => finishedAlarm = value;
        }

        private void Awake()
        {
            finishedAlarm = false; 
            anim = GetComponent<Animator>();
            alarmWaitForSeconds = new WaitForSeconds(alarmInterval);
        }
        Coroutine alertRoutine; 
        public void Scream(Vector3 soundPos)
        {
            if (alertRoutine != null)
            {
                StopCoroutine(alertRoutine);
            }
            alertRoutine = StartCoroutine(MakeAlarm(soundPos));
        }
        public void MakeSound(Vector3 pos)
        {
            Collider[] soundHits = Physics.OverlapSphere(pos, soundIntensity);
            if (soundHits.Length <= 0)
                return;
            foreach (Collider collider in soundHits)
            {
                IHearable hearable = collider.gameObject.GetComponent<IHearable>();
                hearable?.Heard(pos, GameManager.traceSound.WallIntersect(pos, collider.transform.position));
            }
        }

        IEnumerator MakeAlarm(Vector3 soundPos)
        {
            anim.SetTrigger("Scream");
            MakeSound(soundPos); 
            yield return alarmWaitForSeconds; 
            finishedAlarm = true;
        }
    }

}

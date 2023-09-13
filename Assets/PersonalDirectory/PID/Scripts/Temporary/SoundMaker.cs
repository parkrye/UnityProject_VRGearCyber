using PID;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PID
{
    public class SoundMaker : MonoBehaviour
    {
        [SerializeField] float soundIntensity;
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

        public void HitWall(Vector3 muzzlePoint, Vector3 muzzleDirection)
        {
            
        }
    }

}

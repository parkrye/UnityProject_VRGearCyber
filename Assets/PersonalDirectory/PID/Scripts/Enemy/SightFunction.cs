using PID;
using UnityEngine;
using UnityEngine.Events;

namespace ildoo
{
    public class SightFunction : MonoBehaviour
    {
        [SerializeField] LayerMask targetMask;
        [SerializeField] Transform targetEye; 
        Transform PlayerInSight;
        //Should Be Declared by using Robot
        public UnityAction<Transform> PlayerFound;
        //Should be leading to Assualt State 
        public UnityAction PlayerLost;
        //Shoudl be leading to Tracing State 

        float presenceTimer;
        const float maxTimer = 3f;
        public bool TargetFound
        {
            get;
            private set;
        }
        float enemyDetectRange;
        float sightAngle;

        private void Update()
        {
            FindTarget();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!targetMask.Contain(other.gameObject.layer))
            {
                return;
            }
            else
            {
                presenceTimer += Time.deltaTime;
                if (presenceTimer > maxTimer)
                {
                    TargetFound = true;
                    //Should be replaced by the state changing event; 
                    PlayerFound?.Invoke(other.transform); 
                    PlayerInSight = other.transform;
                    presenceTimer = Mathf.Clamp(presenceTimer, 0, maxTimer);
                    return;
                }
            }
            presenceTimer -= Time.deltaTime;
            presenceTimer = Mathf.Clamp(presenceTimer, 0, maxTimer);
        }

        Vector3 dirTarget;
        RaycastHit obstacleHit;
        public void FindTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyDetectRange, targetMask);
            if (colliders.Length == 0)
            {
                PlayerInSight = null;
                TargetFound = false;
                return;
            }
            foreach (Collider collider in colliders)
            {
                dirTarget = collider.transform.position - transform.position;
                dirTarget.y = 0f;
                dirTarget.Normalize();
                // IF Player is found on a given Range, 
                if (Vector3.Dot(transform.forward, dirTarget) > Mathf.Cos(sightAngle * 0.5f * Mathf.Deg2Rad))
                {
                    continue;
                }
                //Start Coroutine for hiding activities. 

                Vector2 distToTarget = (Vector2)collider.transform.position - (Vector2)transform.position;
                float distance = Vector2.SqrMagnitude(distToTarget);
                if (Physics.Raycast(transform.position, dirTarget, out obstacleHit, distance))
                {
                    if (!targetMask.Contain(obstacleHit.collider.gameObject.layer))
                        break;
                    PlayerInSight = obstacleHit.transform;
                    TargetFound = true;
                    PlayerFound?.Invoke(obstacleHit.transform); 
                }
            }
        }
        public bool TargetInValidRange() 
        {
            return false; 
        }
    }
}
using PID;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace PGR
{
    public class NormalCityPeople : MonoBehaviour, IHitable, IStrikable
    {
        [SerializeField] Transform startTransform, endTransform;
        [SerializeField] WalkingPeople walkingPeople;
        [SerializeField] NavMeshAgent agent;
        [SerializeField] bool isAttacked;

        void OnEnable()
        {
            if(agent == null)
                agent = GetComponent<NavMeshAgent>();
            agent.enabled = false;
        }

        public void SetDest(WalkingPeople wp, Transform start, Transform end)
        {
            walkingPeople = wp;
            startTransform = start;
            endTransform = end;
            StartCoroutine(AgentRoutine());
        }

        IEnumerator AgentRoutine()
        {
            yield return new WaitForSeconds(1f);
            agent.enabled = true;
            agent.SetDestination(endTransform.position);

            yield return new WaitUntil(() => Vector3.SqrMagnitude(transform.position - endTransform.position) < 1f);
            agent.enabled = false;
            walkingPeople.PersonGoHome();
            GameManager.Resource.Destroy(this);
        }

        void RunAway()
        {
            if (!isAttacked)
                return;
            isAttacked = true;
            float distA = Vector3.SqrMagnitude(transform.position - endTransform.position);
            float distB = Vector3.SqrMagnitude(transform.position - startTransform.position);

            if(distA < distB)
            {
                agent.SetDestination(endTransform.position);
                agent.speed *= 2f;
            }
            else
            {
                endTransform = startTransform;
                agent.SetDestination(startTransform.position);
                agent.speed *= 2f;
            }
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            RunAway();
        }

        public void TakeStrike(Transform hitter, float damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            RunAway();
        }
    }
}

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace PGR
{
    public class CityCar : MonoBehaviour
    {
        public UnityEvent ArriveEvent;
        public NavMeshAgent agent;
        [SerializeField] int playerCount;
        [SerializeField] float speed;
        [SerializeField] Light pointLight;
        [SerializeField] Color[] colors;

        public void SettingAgent(Vector3 position)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.SetDestination(position);
            pointLight.color = colors[Random.Range(0, colors.Length)];
        }

        void LateUpdate()
        {
            if (agent == null)
                return;

            if (agent.remainingDistance > 0.1f && agent.remainingDistance < 1f)
            {
                ArriveEvent?.Invoke();
                GameManager.Resource.Destroy(this);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                playerCount++;

                if (playerCount > 0)
                {
                    agent.speed = 0f;
                }
                else
                {
                    agent.speed = speed;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                playerCount--;

                if (playerCount > 0)
                {
                    agent.speed = 0f;
                }
                else
                {
                    agent.speed = speed;
                }
            }
        }
    }

}
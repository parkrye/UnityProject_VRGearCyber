using PID;
using UnityEngine;

namespace PGR
{
    public class JustBreakable : MonoBehaviour, IHitable, IStrikable
    {
        [SerializeField] int life;

        void Start()
        {
            if (life <= 0)
                life = 1;
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            life -= damage;
            if (life <= 0)
                Destroyed();
        }

        public void TakeStrike(Transform hitter, float damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            life -= (int)damage;
            if (life <= 0)
                Destroyed();
        }

        void Destroyed()
        {
            GameManager.Resource.Destroy(gameObject);
        }
    }

}
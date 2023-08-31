using UnityEngine;

namespace PGR
{
    public class PlayableObject : CustomGrabInteractable, IHitable
    {
        [SerializeField] int maxHP, nowHP;

        void Start()
        {
            if (maxHP <= 0)
                maxHP = 1;
            nowHP = maxHP;
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            nowHP -= damage;
            if (nowHP <= 0)
                DestroyObject();
        }

        void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}

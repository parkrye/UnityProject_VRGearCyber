using UnityEngine;
using UnityEngine.Events;

namespace PGR
{
    public class PlayerDataModel : MonoBehaviour, IHitable
    {
        [SerializeField] int maxHP, nowHP;
        [SerializeField] float reverseMaxHP;
        [SerializeField] int accessLevel;
        public UnityEvent<float> HPModifyEvent;

        void Start()
        {
            nowHP = maxHP;
            reverseMaxHP = 1 / maxHP;
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            nowHP -= damage;
            if(nowHP <= 0)
            {
                nowHP = 0;
                Die();
            }
            HPModifyEvent?.Invoke(nowHP / maxHP);
        }

        public void GiveHealth(int heal)
        {
            nowHP += heal;
            nowHP = Mathf.Clamp(nowHP, 0, maxHP);
            HPModifyEvent?.Invoke(nowHP / maxHP);
        }

        void Die()
        {
            GameManager.Scene.LoadScene("PGR_DeadScene");
        }
    }
}

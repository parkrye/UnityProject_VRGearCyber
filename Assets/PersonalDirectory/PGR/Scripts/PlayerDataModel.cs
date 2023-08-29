using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGR
{
    public class PlayerDataModel : MonoBehaviour, IHitable
    {
        [SerializeField] int maxHP, nowHP;
        [SerializeField] int accessLevel;

        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            Debug.Log($"Hit {damage}");
        }
    }
}

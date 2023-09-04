using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PID
{
    [CreateAssetMenu(fileName = "EnemyType_", menuName = "Register/EnemyStatWithType")]
    public class EnemyStat : ScriptableObject
    {
        [Header("Base Stat")]

        [SerializeField] public float moveSpeed;
        [SerializeField] public int maxHealth;
        [SerializeField] public float maxSightRange;
        [SerializeField] public float maxSightAngle;
        [SerializeField] public int attackDamage;
        [SerializeField] public int attackRange;
        [SerializeField] public int maxAmmo;
    }
}
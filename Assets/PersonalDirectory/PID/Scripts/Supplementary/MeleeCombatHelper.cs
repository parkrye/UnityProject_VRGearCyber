using UnityEngine;

namespace PID
{
    public static class MeleeCombatHelper
    {
        const float flankThreshold = 0.3f;
        public const int flankDamageMultiplier = 4;
        public static void FlankJudgement(Transform attacker, Transform target,
            float damage, Vector3 hitPoint, Vector3 hitNormal,
            System.Action<float, Vector3, Vector3, bool> flankAttempt)
        {
            if (attacker == null || target == null)
                return;
            Vector3 playerAttackDir = target.position - attacker.position;
            playerAttackDir.y = 0;
            playerAttackDir.Normalize();
            Vector3 enemyLookDir = target.transform.forward;
            if (Vector3.Dot(enemyLookDir, playerAttackDir) > flankThreshold)
            {
                //Debug.Log($"Player has flanked {target.gameObject.name}");
                flankAttempt(damage, hitPoint, hitNormal, true);
            }
            flankAttempt(damage, hitPoint, hitNormal, false);
        }
    }
}
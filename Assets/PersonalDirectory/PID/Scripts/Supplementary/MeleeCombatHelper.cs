using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace PID
{
    public static class MeleeCombatHelper
    {
        const float flankThreshold = -0.3f;
        public static void FlankJudgement(Transform attacker, Transform target, System.Action<Transform, bool> flankAttempt)
        {
            Vector3 playerAttackDir = target.position - attacker.position;
            playerAttackDir.y = 0;
            playerAttackDir.Normalize(); 
            Vector3 enemyLookDir = target.transform.forward;
            if (Vector3.Dot(enemyLookDir, playerAttackDir) > flankThreshold)
            {
                Debug.Log($"Player has flanked {target.gameObject.name}");
                flankAttempt(target, true);
            }
            flankAttempt(target, false);
        }
        //Example Script for meleestrike, 
        //Could Insert Damage float variable to compute base damage based on the swinging velocity 
        public static void TryFlank(Transform target, bool contestSuccess)
        {
            if (contestSuccess)
            {
                IHitable hittable = target.GetComponent<IHitable>();
                //Insert Damage Here 
                hittable?.TakeDamage(0, Vector3.zero, Vector3.zero);
            }
            else
            {
                IHitable hittable = target.GetComponent<IHitable>();
                //Insert Damage Here 
                hittable?.TakeDamage(0, Vector3.zero, Vector3.zero);
            }
        }
    }
}
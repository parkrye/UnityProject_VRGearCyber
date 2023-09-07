using PID;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace PID
{
    public class RagdollController : MonoBehaviour
    {
        int aliveLayer;
        int deadLayer;
        [Header("FSMProperties")]
        BaseEnemy bodyOwner;
        [SerializeField] NavMeshAgent agent;
        [SerializeField] Collider[] actionColliders;
        Animator anim;

        [Header("RagdollProperties")]
        [SerializeField] public Rigidbody[] ragdollBodies;
        [SerializeField] CapsuleCollider[] ragdollColliders;
        [SerializeField] CharacterJoint[] ragdollJoints;
        [SerializeField] BoxCollider[] ragdollBox;
        [SerializeField] SphereCollider ragdollHead;

        [Header("RagDollParty")]
        [SerializeField] float impactIntensity;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            bodyOwner = GetComponent<GuardEnemy>();
            SearchComponentInChildren();
            DisableRagdollProperties();
            aliveLayer = LayerMask.GetMask("Enemy");
            deadLayer = LayerMask.GetMask("EnemyNeutralized");
        }

        private void OnEnable()
        {
            bodyOwner.onDeath += DeathAftermath;
            bodyOwner.OnAndOff += OnAndOff;
        }

        private void OnDisable()
        {
            bodyOwner.onDeath -= DeathAftermath;
            bodyOwner.OnAndOff -= OnAndOff;
        }

        private void SearchComponentInChildren()
        {
            ragdollColliders = GetComponentsInChildren<CapsuleCollider>();
            ragdollJoints = GetComponentsInChildren<CharacterJoint>();
            ragdollBodies = GetComponentsInChildren<Rigidbody>();
        }

        private void DisableRagdollProperties()
        {
            for (int i = 0; i < ragdollBodies.Length; i++)
            {
                ragdollBodies[i].isKinematic = true;
            }
            for (int i = 0; i < ragdollColliders.Length; i++)
            {
                if (i == 0)
                    continue;
                ragdollColliders[i].enabled = false;
            }
            for (int i = 0; i < ragdollBox.Length; i++)
            {
                if (ragdollBox[i].isTrigger)
                    continue;
                ragdollBox[i].enabled = false;
            }
        }

        private void EnableRagDollProperties()
        {
            for (int i = 0; i < ragdollBodies.Length; i++)
            {
                ragdollBodies[i].isKinematic = false;
            }
            for (int i = 0; i < ragdollColliders.Length; i++)
            {
                ragdollColliders[i].enabled = true;
            }
            for (int i = 0; i < ragdollBox.Length; i++)
            {
                if (ragdollBox[i].isTrigger)
                    continue;
                ragdollJoints[i].connectedBody = null;
            }
        }

        public void DeathAftermath(Vector3 hitDir, Vector3 hitPoint)
        {
            //Find 
            Rigidbody hitPart = RobotHelper.NearestHitPart(ragdollBodies, hitPoint);
            StartCoroutine(DeathSimulation(hitDir, hitPoint, hitPart));
        }

        IEnumerator DeathSimulation(Vector3 hitDir, Vector3 hitPoint, Rigidbody impactPoint)
        {
            EnableRagDollProperties();
            yield return new WaitForEndOfFrame();
            impactPoint.AddForceAtPosition(-1 * impactIntensity * hitDir, hitPoint, ForceMode.Impulse);
            //Find the nearest rigidbody with hitpoint 

        }


        public void OnAndOff(bool switchOn)
        {
            if (switchOn)
            {
                SetGameLayerRecursive(gameObject, aliveLayer);
                anim.enabled = true;
                anim.SetTrigger("Revive");
            }
            else
            {
                SetGameLayerRecursive(gameObject, deadLayer);
                anim.enabled = false;
            }
        }
        private void SetGameLayerRecursive(GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            foreach (Transform child in gameObject.transform)
            {
                SetGameLayerRecursive(child.gameObject, layer);
            }
        }
    }
}
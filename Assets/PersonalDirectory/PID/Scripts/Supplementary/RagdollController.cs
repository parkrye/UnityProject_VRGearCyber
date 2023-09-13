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
            bodyOwner = GetComponent<BaseEnemy>();
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
            ragdollBox = GetComponentsInChildren<BoxCollider>();
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
                if (ragdollBodies[i].gameObject.layer == LayerMask.GetMask("Wearable"))
                    return; 
                ragdollBodies[i].isKinematic = false;
            }
            for (int i = 0; i < ragdollColliders.Length; i++)
            {
                if (i == 0)
                    ragdollColliders[i].enabled = false; 
                ragdollColliders[i].enabled = true;
            }
            for (int i = 0; i < ragdollBox.Length; i++)
            {
                if (ragdollBox[i].isTrigger)
                    continue;
                ragdollBox[i].enabled = true;
            }
        }

        public void DeathAftermath(Vector3 hitDir, Vector3 hitPoint)
        {
            //Find 
            Rigidbody hitPart = RobotHelper.NearestHitPart(ragdollBodies, hitPoint);
            if (hitPart != null)
                StartCoroutine(DeathSimulation(hitDir, hitPoint, hitPart));
            else
                StartCoroutine(DeathSimulationDefault());
        }

        IEnumerator DeathSimulation(Vector3 hitDir, Vector3 hitPoint, Rigidbody impactPoint)
        {
            EnableRagDollProperties();
            yield return new WaitForEndOfFrame();
            anim.enabled = false;
            impactPoint.AddForceAtPosition(-1 * impactIntensity * hitDir, hitPoint, ForceMode.Impulse);
            //Find the nearest rigidbody with hitpoint 
        }

        IEnumerator DeathSimulationDefault()
        {
            EnableRagDollProperties();
            yield return new WaitForEndOfFrame();
            anim.enabled = false;
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
            if (!gameObject.tag == "Wearable")
                gameObject.layer = layer;
            foreach (Transform child in gameObject.transform)
            {
                SetGameLayerRecursive(child.gameObject, layer);
            }
        }
    }
}
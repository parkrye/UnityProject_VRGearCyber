using PID;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace PM
{
    public class SyberDoor : MonoBehaviour, IHitable, IStrikable
    {
        [SerializeField] int hp;
        Animator animator;
        NavMeshObstacle obstacle;
        public SyberDoor connetingDoor;
        public enum Arrow { up, down, right, left }
        public Arrow arrow;
        public Vector3 position;
        bool open;
        public bool test;
        private void Start()
        {
            animator = GetComponent<Animator>();
            GameManager.Data.timeScaleEvent.AddListener(TimeScale);
            position = transform.position;
            obstacle = GetComponent<NavMeshObstacle>();
            StartCoroutine(DoorConnet());
        }
        private void Update()
        {
            if (test)
            {
                test = false;
                StartCoroutine(OpenDoor());
            }
        }
        IEnumerator DoorConnet()
        {
            Ray ray = new Ray(transform.position + transform.forward * 2.5f + transform.up, -transform.right * 20f );
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            connetingDoor = hit.transform.GetComponent<SyberDoor>();
            yield return null;
        }

        public IEnumerator Break()
        {
            GameManager.Data.timeScaleEvent.RemoveListener(TimeScale);
            Destroy(gameObject);
            yield return null;
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            Debug.Log("hit");
            hp -= damage;
            if (hp <= 0)
                StartCoroutine(Break());
        }

        public IEnumerator OpenDoor()
        {
            if(!open)
            {
                open = true;
                StartCoroutine(connetingDoor.OpenDoor());
                obstacle.gameObject.SetActive(false);
                animator.SetTrigger("Open");
                yield return null;
            }
        }

        public void TimeScale()
        {
            animator.speed = GameManager.Data.TimeScale;
        }

        public void TakeStrike(Transform hitter, float damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            Debug.Log("strike");
            TakeDamage((int)damage, hitPoint, hitNormal);
        }
    }
}

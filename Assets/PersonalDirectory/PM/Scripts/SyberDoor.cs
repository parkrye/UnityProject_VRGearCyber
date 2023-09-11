using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    public class SyberDoor : MonoBehaviour, IHitable
    {
        [SerializeField] int hp;
        Animator animator;
        public SyberDoor connetingDoor;
        public enum Arrow { up, down, right, left }
        public Arrow arrow;
        public Vector3 position;
        private void Start()
        {
            animator = GetComponent<Animator>();
            GameManager.Data.timeScaleEvent.AddListener(TimeScale);
            position = transform.position;
            DoorConnet();
        }
        private void Update()
        {
            Debug.DrawRay(transform.position + new Vector3(3, 2, 0), -transform.right * 20f, Color.blue);
        }
        public void DoorConnet()
        {
            Ray ray = new Ray(transform.position + new Vector3(3, 2, 0), -transform.right*20f );
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            connetingDoor = hit.transform.GetComponent<SyberDoor>();
        }

        public IEnumerator Break()
        {
            GameManager.Data.timeScaleEvent.RemoveListener(TimeScale);
            Destroy(gameObject);
            yield return null;
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            hp -= damage;
            if (hp <= 0)
                StartCoroutine(Break());
        }

        public IEnumerator OpenDoor()
        {
            StartCoroutine(connetingDoor.OpenDoor());
            animator.SetTrigger("Open");
            yield return null;
        }

        public void TimeScale()
        {
            animator.speed = GameManager.Data.TimeScale;
        }
    }
}

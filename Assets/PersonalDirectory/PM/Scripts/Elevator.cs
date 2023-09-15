using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    public class Elevator : MonoBehaviour
    {
        Animator animator;
        Transform map;
        bool open;
        int floor=1;
        private void Start()
        {
            animator = GetComponent<Animator>();
            map = GameObject.Find("Map").transform;
            transform.parent = null;
        }

        public void Open()
        {
            if(!open) 
            {
                open = true;
                animator.SetTrigger("Open");
            }
            
        }

        public void Close()
        {
            if (open)
            {
                open = false;
                animator.SetTrigger("Close");
            }
        }
        public void Up()
        {
            if (floor == 1)
                StartCoroutine(UpMove());
        }

        public void Down()
        {
            if (floor == 2)
                StartCoroutine (DownMove());
        }

        IEnumerator UpMove()
        {
            map.transform.position += new Vector3(0, -9, 0);
            yield return new WaitForSeconds(2);
            floor = 2;
            Open();
        }

        IEnumerator DownMove()
        {
            map.transform.position += new Vector3(0, 9, 0);
            yield return new WaitForSeconds(2);
            floor = 1;
            Open();
        }
    }

}

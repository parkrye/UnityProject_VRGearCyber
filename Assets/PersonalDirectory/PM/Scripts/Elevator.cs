using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    Animator animator;
    Transform map;
    bool close;
    private void Start()
    {
        animator = GetComponent<Animator>();
        map = transform.parent.parent.parent;
        transform.parent = null;
    }

    private void Open()
    {
        animator.SetTrigger("Open");
    }

    private void Close()
    {
        animator.SetTrigger("Close");
    }

    IEnumerator UpMove()
    {
        map.transform.position += new Vector3(0, -9, 0);
        yield return new WaitForSeconds(2);
        Open();
    }

    IEnumerator DownMove()
    {
        map.transform.position += new Vector3(0, 9, 0);
        yield return new WaitForSeconds(2);
        Open();
    }
}

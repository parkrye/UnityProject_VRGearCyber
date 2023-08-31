using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyberDoor : MonoBehaviour, Hitable, Hackingable
{
    [SerializeField] int hp;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(OpenDoor());
    }

    public IEnumerator Break()
    {
        Destroy(gameObject);
        yield return null;
    }

    public IEnumerator HackingCheck(bool success)
    {
        if (success)
            StartCoroutine(OpenDoor());
        else
        {
            hp += 5;
        }
        yield return null;
    }

    public IEnumerator Hit(int damage)
    {
        hp -= damage;
        if (hp <= 0)
            StartCoroutine(Break());
        yield return null;
    }

    IEnumerator OpenDoor()
    {
        animator.SetTrigger("Open");
        yield return null;
    }
}

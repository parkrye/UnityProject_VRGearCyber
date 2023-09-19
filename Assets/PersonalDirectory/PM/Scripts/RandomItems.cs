using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItems : MonoBehaviour
{
    [SerializeField] GameObject[] items;

    private void Start()
    {
        items = transform.GetComponentsInChildren<GameObject>();

        for(int i = 0; i<items.Length; i++)
        {
            if (Random.Range(0, 3) == 0)
            {
                items[i].gameObject.SetActive(true);
            }
        }
    }
}

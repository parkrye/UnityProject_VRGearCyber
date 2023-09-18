using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "RandomItemList", menuName = "Register/Item")]
public class RandomItem : ScriptableObject
{
    [SerializeField] GameObject[] itemList;
    public GameObject GetItem(int seed)
    {
        return itemList[seed];
    }

    public int GetSeed()
    {
        int seed = Random.Range(0, itemList.Length);
        return seed;
    }

    public Vector3 itemRandomScale (int seed)
    {
        if (seed >= itemList.Length)
        {
            return Vector3.one;
        }
        return itemList[seed].transform.localScale;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "RandomItemList", menuName = "Register/Item")]
public class RandomItem : ScriptableObject
{
    [SerializeField] GameObject[] itemList; 
    public GameObject GetRandomItem()
    {
        int seed = Random.Range(0, itemList.Length); 
        return itemList[seed];
    }
}
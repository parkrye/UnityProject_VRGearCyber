using PGR;
using System.Collections;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    public PlayerController Player {  get { return playerController; } }

    void OnEnable()
    {
        StartCoroutine(FindPlayerRoutine());
    }

    IEnumerator FindPlayerRoutine()
    {
        while(Player == null)
        {
            yield return null;
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
                playerController = playerObj.GetComponent<PlayerController>();
        }
    }
}
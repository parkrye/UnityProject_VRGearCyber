using PGR;
using System.Collections;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    public PlayerController Player {  get { return playerController; } }
    [SerializeField] GameData.GameTimeState timeState;
    public GameData.GameTimeState TimeState { get {  return timeState; } set { timeState = value; } }

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
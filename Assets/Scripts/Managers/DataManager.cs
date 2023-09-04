using PGR;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class DataManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    public PlayerController Player {  get { return playerController; } }
    [SerializeField] float timeScale;
    public float TimeScale { get {  return timeScale; } set { timeScale = value; } }

    void OnEnable()
    {
        timeScale = 1f;
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
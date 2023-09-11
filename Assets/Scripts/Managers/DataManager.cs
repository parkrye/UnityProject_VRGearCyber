using PGR;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;

public class DataManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    public PlayerController Player {  get { return playerController; } }
    [SerializeField] float timeScale;
    public float TimeScale { get {  return timeScale; } set { timeScale = value; } }
    public UnityEvent timeScaleEvent;

    void OnEnable()
    {
        timeScaleEvent = new UnityEvent();
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
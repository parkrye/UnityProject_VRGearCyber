using System.Collections;
using UnityEngine;

namespace PGR
{
    public class CityDirection : MonoBehaviour
    {
        [SerializeField] AudioSource ringAudio;

        void Start()
        {
            StartCoroutine(Routine1());
        }

        IEnumerator Routine1()
        {
            yield return new WaitForSeconds(1f);
            ringAudio.Play();
            yield return new WaitUntil(() => GameManager.Data.Player.IrisSystem.enabled);
            GameManager.Data.Player.Display.ModifyText("Mission : Heist or Break Data of Company");
            yield return new WaitForSeconds(5f);
            GameManager.Data.Player.Display.ModifyText("");
            Destroy(this);
        }
    }

}
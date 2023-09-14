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
            GameManager.Data.Player.Display.ModifyText("Mission : Heist or Crack Data of the Company");
            yield return new WaitUntil(() => GameManager.Data.Player.IrisSystem.isActiveAndEnabled);
            yield return new WaitForSeconds(5f);
            GameManager.Data.Player.Display.ModifyText("");
            Destroy(this);
        }
    }

}
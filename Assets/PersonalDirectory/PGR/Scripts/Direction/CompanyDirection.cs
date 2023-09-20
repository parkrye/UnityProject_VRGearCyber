using System.Collections;
using UnityEngine;

namespace PGR
{
    public class CompanyDirection : MonoBehaviour
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
            GameManager.Data.Player.Display.ModifyText("There is Secret Entrance in Storage Room on 3rd");
            yield return new WaitUntil(() => GameManager.Data.Player.IrisSystem.isActiveAndEnabled);
            yield return new WaitForSeconds(5f);
            GameManager.Data.Player.Display.ModifyText("");
            Destroy(this);
        }
    }
}

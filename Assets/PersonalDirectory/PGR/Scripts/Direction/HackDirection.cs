using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGR
{
    public class HackDirection : MonoBehaviour
    {
        [SerializeField] bool ready;
        [SerializeField] AudioSource ringAudio;

        private void OnTriggerEnter(Collider other)
        {
            if (!ready && other.tag.Equals("Player"))
            {
                StartCoroutine(Routine1());
            }
        }

        IEnumerator Routine1()
        {
            ready = true;
            ringAudio.Play();
            GameManager.Data.Player.Display.ModifyText("Practice Hacking");
            yield return new WaitUntil(() => GameManager.Data.Player.IrisSystem.isActiveAndEnabled);
            yield return new WaitForSeconds(5f);
            GameManager.Data.Player.Display.ModifyText("Press Button X on Left Controller");
            yield return new WaitUntil(() => GameManager.Data.Player.IrisSystem.isActiveAndEnabled);
            yield return new WaitForSeconds(5f);
            GameManager.Data.Player.Display.ModifyText("This is 'Ready Mode'");
            yield return new WaitUntil(() => GameManager.Data.Player.IrisSystem.isActiveAndEnabled);
            yield return new WaitForSeconds(5f);
            GameManager.Data.Player.Display.ModifyText("Aim the Target and Press Button X Again");
            yield return new WaitUntil(() => GameManager.Data.Player.IrisSystem.isActiveAndEnabled);
            yield return new WaitForSeconds(5f);
            GameManager.Data.Player.Display.ModifyText("");

            yield return new WaitForSeconds(3f);
        }
    }

}
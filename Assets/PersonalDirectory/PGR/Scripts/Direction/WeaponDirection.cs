using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGR
{
    public class WeaponDirection : MonoBehaviour
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
            GameManager.Data.Player.Display.ModifyText("You Can Use 3 Equipment Socket");
            yield return new WaitUntil(() => GameManager.Data.Player.IrisSystem.isActiveAndEnabled);
            yield return new WaitForSeconds(5f);
            GameManager.Data.Player.Display.ModifyText("Left and Right Side of Your Waist, and Back");
            yield return new WaitUntil(() => GameManager.Data.Player.IrisSystem.isActiveAndEnabled);
            yield return new WaitForSeconds(5f);
            GameManager.Data.Player.Display.ModifyText("You Can Identify Is Socket Using or Not in Iris Display");
            yield return new WaitUntil(() => GameManager.Data.Player.IrisSystem.isActiveAndEnabled);
            yield return new WaitForSeconds(5f);
            GameManager.Data.Player.Display.ModifyText("Consider and Take Good Choice");
            yield return new WaitUntil(() => GameManager.Data.Player.IrisSystem.isActiveAndEnabled);
            yield return new WaitForSeconds(5f);
            GameManager.Data.Player.Display.ModifyText("");

            yield return new WaitForSeconds(3f);
        }
    }

}
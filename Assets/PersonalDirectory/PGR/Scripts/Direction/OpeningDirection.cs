using System.Collections;
using UnityEngine;

namespace PGR
{
    public class OpeningDirection : MonoBehaviour
    {
        [SerializeField] bool started;
        [SerializeField] AudioSource ringAudio;
        [SerializeField] GameObject canvas;

        private void OnTriggerEnter(Collider other)
        {
            if (!started && other.tag.Equals("Player"))
            {
                StartCoroutine(Routine1());
            }
        }

        IEnumerator Routine1()
        {
            started = true;
            ringAudio.Play();
            canvas.SetActive(true);
            GameManager.Data.Player.Display.ModifyText("Go to 3rd Floor to Prepare Mission");
            yield return new WaitUntil(() => GameManager.Data.Player.IrisSystem.isActiveAndEnabled);
            canvas.SetActive(false);
            yield return new WaitForSeconds(5f);
            GameManager.Data.Player.Display.ModifyText("");
            Destroy(this);
        }
    }
}
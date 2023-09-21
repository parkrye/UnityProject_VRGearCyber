using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace PGR
{
    public class DeadScene : BaseScene
    {
        [SerializeField] GameObject title, text, warning;
        [SerializeField] Transform warningParent;
        [SerializeField] AudioSource sfx;
        [SerializeField] string sceneName;

        protected override IEnumerator LoadingRoutine()
        {
            GameManager.Data.Player.gameObject.SetActive(false);
            title.SetActive(false);
            warning.SetActive(false);
            text.SetActive(false);
            yield return null;
            Progress = 1f;
            StartCoroutine(OpeningRoutine());
        }

        IEnumerator OpeningRoutine()
        {
            yield return new WaitForSeconds(3f);
            sfx.Play();
            title.SetActive(true);

            yield return new WaitForSeconds(1f);
            sfx.Play();
            warning.SetActive(true);

            yield return new WaitForSeconds(2f);
            sfx.Play();
            title.SetActive(false);
            warning.SetActive(false);
            text.SetActive(true);

            yield return new WaitForSeconds(1f);
            GameManager.Data.Player.gameObject.SetActive(true);
            GameManager.Scene.LoadScene(sceneName);
        }
    }
}

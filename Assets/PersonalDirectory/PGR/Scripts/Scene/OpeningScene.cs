using System.Collections;
using UnityEngine;

namespace PGR
{
    public class OpeningScene : MonoBehaviour
    {
        [SerializeField] GameObject title, creators;
        [SerializeField] AudioSource sfx;
        [SerializeField] string sceneName;

        void Start()
        {
            StartCoroutine(OpeningRoutine());
        }

        IEnumerator OpeningRoutine()
        {
            yield return new WaitForSeconds(1f);
            sfx.Play();
            title.SetActive(true);

            yield return new WaitForSeconds(2f);
            sfx.Play();
            creators.SetActive(true);

            yield return new WaitForSeconds(3f);
            PlayerPrefs.SetInt("Load Location", 1);
            GameManager.Scene.LoadScene(sceneName);
        }
    }

}
using System.Collections;
using UnityEngine;

namespace PGR
{
    public class DeadScene : BaseScene
    {
        [SerializeField] GameObject title, text, warningPrefab;
        [SerializeField] Transform warningParent;
        [SerializeField] AudioSource sfx;
        [SerializeField] string sceneName;

        protected override IEnumerator LoadingRoutine()
        {
            GameManager.Data.DestroyPlayer();
            yield return null;
            Progress = 1f;
            StartCoroutine(OpeningRoutine());
        }

        IEnumerator OpeningRoutine()
        {
            yield return new WaitForSeconds(1f);
            sfx.Play();
            title.SetActive(true);

            for(int i = 0; i < 100; i++)
            {
                yield return new WaitForSeconds(0.1f / i);
                sfx.Play();
                GameObject warning = GameManager.Resource.Instantiate(warningPrefab, warningParent);
                warning.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-23f, 23f), Random.Range(-23f, 23f));
            }

            yield return new WaitForSeconds(2f);
            sfx.Play();
            text.SetActive(true);

            yield return new WaitForSeconds(3f);
            GameManager.Scene.LoadScene(sceneName);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    BaseScene curScene;

    public bool ReadyToPlay { get; private set; }

    public BaseScene CurScene
    {
        get
        {
            if (!curScene)
                curScene = GameObject.FindObjectOfType<BaseScene>();

            return curScene;
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadingRoutine(sceneName));
    }

    IEnumerator LoadingRoutine(string sceneName)
    {
        ReadyToPlay = false;
        Time.timeScale = 1f;
        yield return new WaitForSeconds(1f);
        if(GameManager.Data.Player)
            GameManager.Data.Player.LoadingUI.Loading(true);
        AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName);
        while (!oper.isDone)
        {
            yield return null;
        }

        if (CurScene)
        {
            CurScene.LoadAsync();
            while (CurScene.Progress < 1f)
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(1f);
        ReadyToPlay = true;
    }
}
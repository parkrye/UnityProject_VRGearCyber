using System.Collections;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    LoadingUI loadingUI;
    Canvas loadingCanvas;

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

    void Awake()
    {
        loadingCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        loadingCanvas.gameObject.name = "LoadingCanvas";
        loadingCanvas.sortingOrder = 10;
        loadingCanvas.transform.SetParent(transform, false);

        LoadingUI _loadingUI = Resources.Load<LoadingUI>("UI/LoadingUI");
        loadingUI = GameManager.Resource.Instantiate(_loadingUI);
        loadingUI.transform.SetParent(loadingCanvas.transform, false);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadingRoutine(sceneName));
    }

    IEnumerator LoadingRoutine(string sceneName)
    {
        ReadyToPlay = false;
        Time.timeScale = 1f;
        loadingUI.SetProgress(0f);
        loadingUI.FadeIn();
        yield return new WaitForSeconds(1f);

        AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName);
        while (!oper.isDone)
        {
            loadingUI.SetProgress(Mathf.Lerp(0.0f, 0.5f, oper.progress));
            yield return null;
        }

        if (CurScene)
        {
            CurScene.LoadAsync();
            while (CurScene.Progress < 1f)
            {
                loadingUI.SetProgress(Mathf.Lerp(0.5f, 1f, CurScene.Progress));
                yield return null;
            }
        }

        loadingUI.SetProgress(1f);
        loadingUI.FadeOut();
        yield return new WaitForSeconds(1f);
        ReadyToPlay = true;
    }
}
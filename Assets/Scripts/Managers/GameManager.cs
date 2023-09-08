using PID;
using System.Resources;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    static PoolManager poolManager;
    static ResourceManager resourceManager;
    static DataManager dataManager;
    static SceneManager sceneManager;
    static AudioManager audioManager;
    static TraceableSoundManager traceManager; 

    public static GameManager Instance { get { return instance; } }
    public static PoolManager Pool { get { return poolManager; } }
    public static ResourceManager Resource { get { return resourceManager; } }
    public static DataManager Data { get { return dataManager; } }
    public static SceneManager Scene { get { return sceneManager; } }
    public static AudioManager Audio { get { return audioManager; } }
    public static TraceableSoundManager traceSound {  get { return traceManager; } }

    void Awake()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
        InitManagers();
    }

    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    void InitManagers()
    {
        GameObject resourceObj = new GameObject();
        resourceObj.name = "ResourceManager";
        resourceObj.transform.parent = transform;
        resourceManager = resourceObj.AddComponent<ResourceManager>();

        GameObject poolObj = new GameObject();
        poolObj.name = "PoolManager";
        poolObj.transform.parent = transform;
        poolManager = poolObj.AddComponent<PoolManager>();

        GameObject dataObj = new GameObject();
        dataObj.name = "DataManager";
        dataObj.transform.parent = transform;
        dataManager = dataObj.AddComponent<DataManager>();

        GameObject sceneObj = new GameObject();
        sceneObj.name = "SceneManager";
        sceneObj.transform.parent = transform;
        sceneManager = sceneObj.AddComponent<SceneManager>();

        GameObject audioObj = new GameObject();
        audioObj.name = "AudioManager";
        audioObj.transform.parent = transform;
        audioManager = audioObj.AddComponent<AudioManager>();

        GameObject traceObj = new GameObject();
        traceObj.name = "TraceableSoundManager";
        traceObj.transform.parent = transform;
        traceManager = traceObj.AddComponent<TraceableSoundManager>();
    }
}
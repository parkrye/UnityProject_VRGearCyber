using System.Collections;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    public float Progress { get; protected set; }
    protected abstract IEnumerator LoadingRoutine();

    public void LoadAsync()
    {
        StartCoroutine(LoadingRoutine());
    }
}
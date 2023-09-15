using PM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerComputer : MonoBehaviour, IHackable
{
    [SerializeField] GameData.HackProgressState state;
    [SerializeField] int pairCount;
    [SerializeField] int fixedPointPerPairCount;
    LightOn[] lights;
    MaterialChange[] materials;
    public Transform Robots;
    float timeLimit;
    private void Start()
    {
        lights = GameObject.Find("Map").GetComponentsInChildren<LightOn>();
        materials = transform.GetComponentsInChildren<MaterialChange>();
        timeLimit = 60f;
    }

    private void LightOn()
    {
        foreach (LightOn light in lights)
        {
            light.LightSet();
        }
    }

    private void HackingStart()
    {
        foreach(MaterialChange material in materials)
        {
            material.HackingStart();
        }
    }

    private void HackingStop()
    {
        foreach (MaterialChange material in materials)
        {
            material.HackingStop();
        }
    }
    /// <summary>
    /// Call by Hacking Cable
    /// </summary>
    public virtual void Hack()
    {
        StartCoroutine(WaitingHackResultRoutine());
    }

    public virtual void ChangeProgressState(GameData.HackProgressState value)
    {
        state = value;
    }

    /// <summary>
    /// if hacking failed run
    /// </summary>
    public virtual void Failure()
    {
        Robots.gameObject.SetActive(true);
    }

    /// <summary>
    /// if hacking success run
    /// </summary>
    public virtual void Success()
    {
        LightOn();
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        while (timeLimit > 0)
        {
            timeLimit -= Time.deltaTime;
            GameManager.Data.Player.Display.ModifyText($"Escape In Time: {timeLimit:##.##}");
            yield return null;
        }
        GameManager.Data.Player.Data.TakeDamage(1000, new Vector3(0,0,0), new Vector3(0, 0, 0));
        yield return null;
    }
    /// <summary>
    /// Wait until hack progress state be failure or success
    /// </summary>
    public virtual IEnumerator WaitingHackResultRoutine()
    {
        yield return null;
        state = GameData.HackProgressState.Progress;
        HackingStart();
        yield return new WaitUntil(() =>  state != GameData.HackProgressState.Progress);
        HackingStop();
        switch(state)
        {
             case GameData.HackProgressState.Failure:
                 Failure();
                 break;
             case GameData.HackProgressState.Success:
                 Success();
                 break;
        }
    }

    public (int, int) GetDifficulty()
    {
        return (pairCount, fixedPointPerPairCount);
    }
}

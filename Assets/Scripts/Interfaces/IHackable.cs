using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHackable
{
    //[SerializeField] GameData.HackProgressState state;

    /// <summary>
    /// Call by Hacking Cable
    /// </summary>
    public virtual void Hack()
    {
        // StartCoroutine(WaitingHackResultRoutine());
    }

    public virtual void ChangeProgressState(GameData.HackProgressState value)
    {
        // state = value;
    }

    /// <summary>
    /// if hacking failed run
    /// </summary>
    public virtual void Failure()
    {

    }

    /// <summary>
    /// if hacking success run
    /// </summary>
    public virtual void Success()
    {

    }
 
    /// <summary>
    /// Wait until hack progress state be failure or success
    /// </summary>
    public virtual IEnumerator WaitingHackResultRoutine()
    {
        yield return null;
        // state = GameData.HackProgressState.Progress;
        // yield return WaitUntil(() => { state != GameData.HackProgressState.Progress });
        // switch(state)
        // {
        //      case GameData.HackProgressState.Failure:
        //          Failure();
        //          break;
        //      case GameData.HackProgressState.Success:
        //          Success();
        //          break;
        // }
    }

}

using System.Collections;
using UnityEngine;

namespace PGR
{
    public class FixedLocationScene : BaseScene
    {
        [SerializeField] Transform[] startPositions;

        protected override IEnumerator LoadingRoutine()
        {
            GameManager.Data.Player.LoadingUI.Loading(true);
            GameManager.Data.Player.MoveTransform(startPositions[PlayerPrefs.GetInt("Load Location")].position);
            yield return new WaitForSeconds(3f);
            GameManager.Data.Player.LoadingUI.Loading(false);
            Progress = 1f;
        }
    }
}
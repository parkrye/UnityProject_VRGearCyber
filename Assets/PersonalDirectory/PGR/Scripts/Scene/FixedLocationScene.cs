using System.Collections;
using UnityEngine;

namespace PGR
{
    public class FixedLocationScene : BaseScene
    {
        [SerializeField] Transform[] startPositions;

        protected override IEnumerator LoadingRoutine()
        {
            yield return null;
            GameManager.Data.Player.MoveTransform(startPositions[PlayerPrefs.GetInt("Load Location")].position);
            Progress = 1f;
        }
    }

}
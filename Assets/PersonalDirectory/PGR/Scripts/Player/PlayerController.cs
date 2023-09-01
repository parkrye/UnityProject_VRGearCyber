using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] PlayerDataModel playerDataModel;
        [SerializeField] PlayerHandMotion playerHandMotion;
        [SerializeField] PlayerHandWallCheck playerLeftWallCheck, playerRightWallCheck;
        [SerializeField] Camera irisSystemCamera;
        public PlayerDataModel Data { get { return playerDataModel; } }
        public PlayerHandMotion HandMotion { get { return playerHandMotion; } }
        public PlayerHandWallCheck LeftWall { get {  return playerLeftWallCheck; } }
        public PlayerHandWallCheck RightWall {  get { return playerRightWallCheck; } }
        public Camera IrisSystem { get { return irisSystemCamera; } }

        [SerializeField] bool turnType;
        public bool TurnType { get { return turnType; } set { ChangeTurnType(value); } }

        [SerializeField] ActionBasedContinuousTurnProvider continuousTurnProvider;
        [SerializeField] ActionBasedSnapTurnProvider snapTurnProvider;

        void ChangeTurnType(bool isSmooth)
        {
            turnType = isSmooth;

            continuousTurnProvider.gameObject.SetActive(isSmooth);
            snapTurnProvider.gameObject.SetActive(!isSmooth);
        }
    }

}
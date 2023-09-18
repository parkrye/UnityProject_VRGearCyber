using System.Collections;
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
        [SerializeField] PlayerSight playerSight;
        [SerializeField] IrisSystemDisplay irisSystemDisplay;
        [SerializeField] Transform xROriginTransform;
        [SerializeField] PlayerExtraInput playerExtraInput;
        [SerializeField] LoadingUI loadingUI;

        public PlayerDataModel Data { get { return playerDataModel; } }
        public PlayerHandMotion HandMotion { get { return playerHandMotion; } }
        public PlayerHandWallCheck LeftWall { get {  return playerLeftWallCheck; } }
        public PlayerHandWallCheck RightWall {  get { return playerRightWallCheck; } }
        public Camera IrisSystem { get { return irisSystemCamera; } }
        public PlayerSight Sight { get { return playerSight; } }
        public IrisSystemDisplay Display { get { return irisSystemDisplay; } }
        public PlayerExtraInput ExtraInput { get {  return playerExtraInput; } }
        public LoadingUI LoadingUI { get { return loadingUI; } }

        [SerializeField] ActionBasedContinuousTurnProvider continuousTurnProvider;
        [SerializeField] ActionBasedSnapTurnProvider snapTurnProvider;

        void Start()
        {
            if (GameManager.Data.Player != null)
            {
                Destroy(gameObject);
                return;
            }
            StartCoroutine(AwakeWaitingRoutine());
        }

        IEnumerator AwakeWaitingRoutine()
        {
            yield return new WaitForSeconds(1f);
            Data.HPModifyEvent.AddListener(Display.ModifyHP);
            Display.ModifyText("");
            DontDestroyOnLoad(gameObject);
        }

        public void ChangeTurnType(bool isSmooth)
        {
            continuousTurnProvider.gameObject.SetActive(isSmooth);
            snapTurnProvider.gameObject.SetActive(!isSmooth);
        }

        public void MoveTransform(Vector3 position)
        {
            xROriginTransform.position = position;
        }
    }

}
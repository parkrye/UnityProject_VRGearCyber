using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] PlayerDataModel playerDataModel;
        public PlayerDataModel Data { get { return playerDataModel; } }

        [SerializeField] bool moveType, turnType;
        public bool MoveType { get { return moveType; } set{ ChangeMoveType(value); } }
        public bool TurnType { get { return turnType; } set { ChangeTurnType(value); } }

        [SerializeField] ActionBasedContinuousMoveProvider continuousMoveProvider;
        [SerializeField] TeleportationProvider teleportationProvider;

        [SerializeField] ActionBasedContinuousTurnProvider continuousTurnProvider;
        [SerializeField] ActionBasedSnapTurnProvider snapTurnProvider;

        void ChangeMoveType(bool isSmooth)
        {
            moveType = isSmooth;
            continuousMoveProvider.enabled = isSmooth;
            teleportationProvider.enabled = !isSmooth;
        }

        void ChangeTurnType(bool isSmooth)
        {
            turnType = isSmooth;

            continuousTurnProvider.enabled = isSmooth;
            snapTurnProvider.enabled = !isSmooth;
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerController : MonoBehaviour
{
    [SerializeField] bool moveType, turnType;
    public bool MoveType { get { return moveType; } }
    public bool TurnType { get { return turnType; } }

    [SerializeField] ActionBasedContinuousMoveProvider continuousMoveProvider;
    [SerializeField] TeleportationProvider teleportationProvider;

    [SerializeField] ActionBasedContinuousTurnProvider continuousTurnProvider;
    [SerializeField] ActionBasedSnapTurnProvider snapTurnProvider;

    public void ChangeMoveType(bool isSmooth)
    {
        moveType = isSmooth;
        continuousMoveProvider.enabled = isSmooth;
        teleportationProvider.enabled = !isSmooth;
    }

    public void ChangeTurnType(bool isSmooth)
    {
        turnType = isSmooth;

        continuousTurnProvider.enabled = isSmooth;
        snapTurnProvider.enabled = !isSmooth;
    }
}

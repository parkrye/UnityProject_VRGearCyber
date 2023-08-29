using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] PlayerDataModel playerDataModel;
        public PlayerDataModel Data { get { return playerDataModel; } }

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
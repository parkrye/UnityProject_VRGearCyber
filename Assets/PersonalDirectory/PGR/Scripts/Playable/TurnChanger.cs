using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGR
{
    public class TurnChanger : MonoBehaviour
    {
        [SerializeField] GameObject table;

        public void OnMonitorButtonClicked()
        {
            table.SetActive(true);
        }

        public void OnSmoothButtonClicked()
        {
            GameManager.Data.Player.ChangeTurnType(false);
            table.SetActive(false);
        }

        public void OnSnapButtonClicked()
        {
            GameManager.Data.Player.ChangeTurnType(true);
            table.SetActive(false);
        }
    }
}

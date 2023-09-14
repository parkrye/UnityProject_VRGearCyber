using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGR; 

namespace PID
{
    public class EnemyManager : MonoBehaviour
    {
        //Static Class, 
        //Manages, and alerts enemies with game status based on Player's progression. 
        PlayerDataModel playerHealthAccess;
        private void Start()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player"); 
            if (playerObj == null)
            {
                Debug.Log("Player Not Found"); 
            }
            else
            {
                playerHealthAccess = playerObj.GetComponentInChildren<PlayerDataModel>();
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public bool exit;
    public void ExitOn()
    {
        exit = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (exit)
            GameManager.Scene.LoadScene("PGR_EndingScene");
    }
}

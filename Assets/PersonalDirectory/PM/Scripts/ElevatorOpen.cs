using PM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorOpen : MonoBehaviour
{
    public Elevator elevator;

    public void Open()
    {
        Debug.Log("Open");
        elevator.Open();
    }
}

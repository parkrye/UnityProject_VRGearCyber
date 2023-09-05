using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public int x;
    public int y;
    public bool right;
    public bool down;
    private void Start()
    {
        right = false; 
        down = false;
    }
}

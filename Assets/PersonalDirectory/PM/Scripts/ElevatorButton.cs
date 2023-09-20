using PM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PM
{
    public class ElevatorButton : MonoBehaviour
    {
        public enum Button {up, down}
        public Elevator elevator;
        public Button button; 

        public void Click()
        {
            if(button == Button.up)
            {
                elevator.Up();
            }
            else if(button == Button.down)
            {
                elevator.Down();
            }
        }
    }
}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PGR
{
    public class IrisSystemController : MonoBehaviour
    {
        [SerializeField] bool isRight, isPressed;
        public bool IsRight { get { return isRight; } }

        public UnityEvent<bool> TriggerEvent;

        void OnLeftTriggerGrab(InputValue inputValue)
        {
            if (isRight)
                return;

            isPressed = !isPressed;
            if(isPressed)
                TriggerEvent?.Invoke(isRight);
        }

        void OnRightTriggerGrab(InputValue inputValue)
        {
            if (!isRight)
                return;

            isPressed = !isPressed;
            if (isPressed)
                TriggerEvent?.Invoke(isRight);
        }
    }
}

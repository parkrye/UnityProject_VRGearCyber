using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PGR
{
    public class PlayerExtraInput : MonoBehaviour
    {
        public UnityEvent<bool> LeftHandPrimaryButtonEvent, LeftHandSecondaryButtonEvent, LeftHandTriggerGrabEvent,
                                RightHandPrimaryButtonEvent, RightHandSecondaryButtonEvent, RightHandTriggerGrabEvent;

        void OnLeftPrimaryButton(InputValue inputValue)
        {
            LeftHandPrimaryButtonEvent?.Invoke(inputValue.isPressed);
        }

        void OnLeftSecondaryButton(InputValue inputValue)
        {
            LeftHandSecondaryButtonEvent?.Invoke(inputValue.isPressed);
        }

        void OnLLeftTriggerGrab(InputValue inputValue)
        {
            LeftHandTriggerGrabEvent?.Invoke(inputValue.isPressed);
        }

        void OnRightPrimaryButton(InputValue inputValue)
        {
            RightHandPrimaryButtonEvent?.Invoke(inputValue.isPressed);
        }

        void OnRightSecondaryButton(InputValue inputValue)
        {
            RightHandSecondaryButtonEvent?.Invoke(inputValue.isPressed);
        }

        void OnRightTriggerGrab(InputValue inputValue)
        {
            RightHandTriggerGrabEvent?.Invoke(inputValue.isPressed);
        }
    }

}
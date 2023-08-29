using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandMotion : MonoBehaviour
{
    [SerializeField] Animator lAnim, rAnim;

    void OnLeftGrab(InputValue inputValue)
    {
        lAnim.SetBool("Grab", inputValue.isPressed);
    }

    void OnLeftTrigger(InputValue inputValue)
    {
        lAnim.SetBool("Trigger", inputValue.isPressed);
    }

    void OnLeftThumb(InputValue inputValue)
    {
        lAnim.SetBool("Thumb", inputValue.isPressed);
    }

    void OnRightGrab(InputValue inputValue)
    {
        rAnim.SetBool("Grab", inputValue.isPressed);
    }

    void OnRightTrigger(InputValue inputValue)
    {
        rAnim.SetBool("Trigger", inputValue.isPressed);
    }

    void OnRightThumb(InputValue inputValue)
    {
        rAnim.SetBool("Thumb", inputValue.isPressed);
    }
}

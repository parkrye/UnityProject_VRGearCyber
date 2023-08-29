using UnityEngine;
using UnityEngine.InputSystem;

namespace PGR
{
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

        public void GrabAboutGun(bool isGrab)
        {
            rAnim.SetBool("Gun", isGrab);
        }

        public void TriggerGun(bool isTrigger)
        {
            rAnim.SetBool("Shot", isTrigger);
        }

        public void GrabOnBat(bool isRight, bool isGrab)
        {
            if (isRight)
                rAnim.SetBool("Bat", isGrab);
            else
                lAnim.SetBool("Bat", isGrab);
        }

        public void WallCheck(bool isRight, bool isCheck)
        {
            if (isRight)
                rAnim.SetBool("Wall", isCheck);
            else
                lAnim.SetBool("Wall", isCheck);
        }
    }
}
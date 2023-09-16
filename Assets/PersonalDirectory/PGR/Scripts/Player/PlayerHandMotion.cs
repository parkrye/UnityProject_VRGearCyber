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

        public void GrabOnGunRight(bool isGrab)
        {
            rAnim.SetBool("Gun", isGrab);
        }

        public void GrabOnGunLeft(bool isGrab)
        {
            lAnim.SetBool("Gun", isGrab);
        }

        public void TriggerGunRight(bool isTrigger)
        {
            rAnim.SetBool("Shot", isTrigger);
        }

        public void TriggerGunLeft(bool isTrigger)
        {
            lAnim.SetBool("Shot", isTrigger);
        }

        public void GrabOnCloseWeaponRight(bool isGrab)
        {
            rAnim.SetBool("CloseWeapon", isGrab);
        }

        public void GrabOnCloseWeaponLeft(bool isGrab)
        {
            lAnim.SetBool("CloseWeapon", isGrab);
        }

        public void WallCheck(bool isRight, bool isCheck)
        {
            if (isRight)
                rAnim.SetBool("Wall", isCheck);
            else
                lAnim.SetBool("Wall", isCheck);
        }

        public void CatchObjectRight(bool isGrab)
        {
            rAnim.SetBool("Catch", isGrab);
        }

        public void CatchObjectLeft(bool isGrab)
        {
            lAnim.SetBool("Catch", isGrab);
        }

        public bool IsNothingInHand(bool isRight)
        {
            if (isRight)
                return rAnim.GetBool("Gun") || rAnim.GetBool("CloseWeapon") || rAnim.GetBool("Wall") || rAnim.GetBool("Catch");
            else
                return lAnim.GetBool("Gun") || lAnim.GetBool("CloseWeapon") || lAnim.GetBool("Wall") || lAnim.GetBool("Catch");
        }
    }
}
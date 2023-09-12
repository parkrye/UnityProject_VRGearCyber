using UnityEngine;

namespace PGR
{
    public class IrisSystemDevice : MonoBehaviour
    {
        [SerializeField] bool isHandOnArea, isRight, isLeft, isOn;
        [SerializeField] GameObject irisSystemCamera;

        void OnEnable()
        {
            irisSystemCamera.SetActive(false);
        }

        void OnTriggerEnter(Collider other)
        {
            IrisSystemController controller = other.GetComponent<IrisSystemController>();
            if (controller == null)
                return;

            isHandOnArea = true;
            if(controller.IsRight)
                isRight = true;
            else
                isLeft = true;
        }

        void OnTriggerExit(Collider other)
        {
            IrisSystemController controller = other.GetComponent<IrisSystemController>();
            if (controller == null)
                return;

            if (controller.IsRight)
                isRight = false;
            else
                isLeft = false;

            if(!isRight && !isLeft)
                isHandOnArea = false;
        }

        public void OnTriggerEnterEvent(bool _isRight)
        {
            if (!isHandOnArea)
                return;

            if (isRight != _isRight && isLeft != _isRight)
                return;

            isOn = !isOn;
            TurnIrisSystem(isOn);
        }

        public void TurnIrisSystem(bool value)
        {
            irisSystemCamera.SetActive(value);
        }
    }
}
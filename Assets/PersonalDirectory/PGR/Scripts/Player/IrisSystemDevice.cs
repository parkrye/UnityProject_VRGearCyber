using UnityEngine;

namespace PGR
{
    public class IrisSystemDevice : MonoBehaviour
    {
        [SerializeField] bool isHandOnArea, isRight, isLeft, isOn, forcedOn;
        [SerializeField] GameObject irisSystemCamera;
        [SerializeField] AudioSource onAudio, offAudio;

        void OnEnable()
        {
            irisSystemCamera.SetActive(false);
        }

        void OnTriggerEnter(Collider other)
        {
            if (forcedOn)
                return;

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
            if (forcedOn)
                return;

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

            if (isRight != _isRight && isLeft != !_isRight)
                return;

            isOn = !isOn;
            TurnIrisSystem(isOn);
        }

        public void TurnIrisSystem(bool value)
        {
            if(value)
                onAudio.Play();
            else
                offAudio.Play();
            irisSystemCamera.SetActive(value);
        }

        public void ForceOn(bool value)
        {
            forcedOn = value;
            if (forcedOn)
            {
                onAudio.Play();
                irisSystemCamera.SetActive(true);
                isOn = true;
            }
            else
            {
                offAudio.Play();
                irisSystemCamera.SetActive(false);
                isOn = false;
            }
        }
    }
}
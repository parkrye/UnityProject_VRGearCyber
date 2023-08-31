using UnityEngine;

namespace PGR
{
    public class IrisSystemDevice : MonoBehaviour
    {
        [SerializeField] bool isHandOnArea, isRight, isOn;
        [SerializeField] GameObject irisSystemCanvas;

        void OnEnable()
        {
            irisSystemCanvas.SetActive(false);
        }

        void OnTriggerEnter(Collider other)
        {
            IrisSystemController controller = other.GetComponent<IrisSystemController>();
            if (controller == null)
                return;

            isHandOnArea = true;
            isRight = controller.IsRight;
        }

        void OnTriggerExit(Collider other)
        {
            IrisSystemController controller = other.GetComponent<IrisSystemController>();
            if (controller == null)
                return;

            isHandOnArea = false;
            isRight = !controller.IsRight;
        }

        public void OnTriggerEnterEvent(bool _isRight)
        {
            if (!isHandOnArea)
                return;

            if (isRight != _isRight)
                return;

            isOn = !isOn;
            irisSystemCanvas.SetActive(isOn);
        }
    }
}
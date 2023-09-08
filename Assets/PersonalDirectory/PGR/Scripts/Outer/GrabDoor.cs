using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PFR
{
    public class GrabDoor : MonoBehaviour
    {
        [SerializeField] Transform handleOriginTransform;
        [SerializeField] Rigidbody handleRb;
        [SerializeField] GameObject grabableHandle;

        void FixedUpdate()
        {
            if (handleRb == null)
                return;
            handleRb.AddForce(grabableHandle.transform.position - handleRb.transform.position, ForceMode.Force);
        }

        public void EndGrabHandle(HoverExitEventArgs args)
        {
            grabableHandle.transform.position = handleOriginTransform.position;
            grabableHandle.transform.localEulerAngles = Vector3.zero;
        }
    }
}

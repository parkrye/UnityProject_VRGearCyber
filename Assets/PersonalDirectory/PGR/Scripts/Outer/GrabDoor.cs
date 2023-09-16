using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PFR
{
    public class GrabDoor : MonoBehaviour
    {
        [SerializeField] Transform handleOriginTransform;
        [SerializeField] Rigidbody handleRb;
        [SerializeField] GameObject grabableHandle;
        [SerializeField] bool isGrabbed;
        [SerializeField] AudioSource doorAudio;

        void OnEnable()
        {
            doorAudio = GameManager.Resource.Instantiate<AudioSource>("Audio/DoorClip", transform);
        }

        void FixedUpdate()
        {
            if (handleRb == null)
                return;
            if(isGrabbed)
                handleRb.AddForce((grabableHandle.transform.position - handleRb.transform.position) * 10f, ForceMode.Force);
        }

        public void StartGrabHandle(SelectEnterEventArgs args)
        {
            isGrabbed = true;
            doorAudio.Play();
        }

        public void EndGrabHandle(SelectExitEventArgs args)
        {
            isGrabbed = false;
            grabableHandle.transform.position = handleOriginTransform.position;
            grabableHandle.transform.localEulerAngles = Vector3.zero;
        }
    }
}

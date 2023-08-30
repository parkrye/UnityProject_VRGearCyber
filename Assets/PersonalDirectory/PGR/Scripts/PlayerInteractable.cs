using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class PlayerInteractable : XRGrabInteractable
    {
        [Header("Player Interactable Parameters")]
        [SerializeField][Range(1, 10)] uint grabPriority;
        [SerializeField] protected Rigidbody rb;
        [SerializeField] GameData.InteractableType interactableType;
        public GameData.InteractableType InteractableType { get { return interactableType; } }

        public uint Priority 
        { 
            get 
            { 
                return grabPriority; 
            } 
            set 
            {
                if (value <= 0)
                    grabPriority = 1;
                else if (value > 10)
                    grabPriority = 10;
                else
                    grabPriority = value; 
            } 
        }

        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            rb.isKinematic = false;
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            rb.isKinematic = false;
        }
    }

}
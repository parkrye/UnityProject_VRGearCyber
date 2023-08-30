using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class XRExclusiveSocketInteractor : XRSocketInteractor
    {
        [Header("Exclusive Socket Interactor Parameters")]
        [SerializeField] GameData.InteractableType acceptedType;
        public GameData.InteractableType AcceptedType { get { return acceptedType; } }

        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            PlayerInteractable socketTarget = interactable.transform.GetComponent<PlayerInteractable>();

            if (socketTarget == null)
                return false;

            return base.CanSelect(interactable) && (socketTarget.InteractableType == AcceptedType);
        }

        public override bool CanHover(IXRHoverInteractable interactable)
        {
            return CanSelect(interactable as IXRSelectInteractable);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGR;
using UnityEngine.XR.Interaction.Toolkit;

namespace PID
{
    public class RobotCustomItemSocketInteractor : XRSocketInteractor
    {
        [SerializeField] GameObject hoveringItem;

        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            CustomGrabInteractable socketTarget = interactable.transform.GetComponent<CustomGrabInteractable>();

            if (socketTarget == null)
                return false;

            return base.CanSelect(interactable); 
        }

        public override bool CanHover(IXRHoverInteractable interactable)
        {
            return CanSelect(interactable as IXRSelectInteractable);
        }

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            base.OnHoverEntered(args);
        }
        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            base.OnHoverExited(args);
        }
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
        }
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
        }

        IEnumerator Enlarge()
        {
            yield return null; 
        }
    }
}
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class MovablePoint : CustomGrabInteractable
    {
        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            if (args.interactorObject.transform.GetComponent<LineRenderer>())
                return;
            base.OnHoverEntered(args);
        }
    }

}
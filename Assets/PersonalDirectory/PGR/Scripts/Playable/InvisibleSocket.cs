using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class InvisibleSocket : XRExclusiveSocketInteractor
    {
        [Header("Invisible Socket Parameters")]
        [SerializeField] bool isInvisible;
        public void OnSelectEnterEvent(SelectEnterEventArgs args)
        {
            args.interactableObject.transform.SetParent(transform);

            if (!isInvisible)
                return;

            if (args == null)
                return;

            Renderer renderer = args.interactableObject.transform.GetComponentInChildren<Renderer>();
            if (renderer == null)
                return;

            renderer.enabled = false;
        }

        public void OnSelectExitEvent(SelectExitEventArgs args)
        {
            args.interactableObject.transform.SetParent(null);

            if (!isInvisible)
                return;

            if (args == null)
                return;

            Renderer renderer = args.interactableObject.transform.GetComponentInChildren<Renderer>();
            if (renderer == null)
                return;

            renderer.enabled = true;
        }
    }
}
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class InvisibleSocket : XRExclusiveSocketInteractor
    {
        [Header("Invisible Socket Parameters")]
        [SerializeField] bool isInvisible, isStored, isPlayerSocket;
        [SerializeField] int playerSocketNum;
        Renderer[] rd;

        public void OnHoverEnterEvent(HoverEnterEventArgs args)
        {
            if (isStored && 1 << args.interactableObject.transform.gameObject.layer == LayerMask.GetMask("Player Hand"))
            {
                foreach (Renderer r in rd)
                    r.enabled = true;
            }
        }

        public void OnHoverExitEvent(HoverExitEventArgs args)
        {
            if (isStored && 1 << args.interactableObject.transform.gameObject.layer == LayerMask.GetMask("Player Hand"))
            {
                foreach (Renderer r in rd)
                    r.enabled = false;
            }
        }

        public void OnSelectEnterEvent(SelectEnterEventArgs args)
        {
            args.interactableObject.transform.SetParent(transform);

            if (!isInvisible)
                return;

            if (!isStored)
            {
                rd = args.interactableObject.transform.GetComponentsInChildren<Renderer>();
                if (rd == null)
                    return;

                foreach (Renderer r in rd)
                    r.enabled = false;
                isStored = true;
                if (isPlayerSocket)
                {
                    GameManager.Data.Player.Display.UseSocket(playerSocketNum, true);
                }
            }
        }

        public void OnSelectExitEvent(SelectExitEventArgs args)
        {
            args.interactableObject.transform.SetParent(null);

            if (!isInvisible)
                return;

            if (isStored)
            {
                rd = args.interactableObject.transform.GetComponentsInChildren<Renderer>();
                if (rd == null)
                    return;

                foreach (Renderer r in rd)
                    r.enabled = true;
                isStored = false;
                if (isPlayerSocket)
                {
                    GameManager.Data.Player.Display.UseSocket(playerSocketNum, false);
                }
            }
        }
    }
}
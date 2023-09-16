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
        public void OnSelectEnterEvent(SelectEnterEventArgs args)
        {
            if (!isInvisible)
                return;

            if (!isStored)
            {
                rd = args.interactableObject.transform.GetComponentsInChildren<Renderer>();
                if (rd == null)
                    return;

                args.interactableObject.transform.SetParent(transform);
                foreach (Renderer r in rd)
                    r.enabled = false;
                isStored = true;
                if (isPlayerSocket)
                {
                    GameManager.Data.Player.Display.UseSocket(playerSocketNum, true);
                }
            }
            else
            {
                if(1 << args.interactableObject.transform.gameObject.layer == LayerMask.GetMask("Player Hand"))
                {
                    foreach (Renderer r in rd)
                        r.enabled = true;
                }
            }
        }

        public void OnSelectExitEvent(SelectExitEventArgs args)
        {
            if (!isInvisible)
                return;

            if (isStored)
            {
                if (1 << args.interactableObject.transform.gameObject.layer == LayerMask.GetMask("Player Hand"))
                {
                    foreach (Renderer r in rd)
                        r.enabled = false;
                }
                else
                {
                    rd = args.interactableObject.transform.GetComponentsInChildren<Renderer>();
                    if (rd == null)
                        return;

                    args.interactableObject.transform.SetParent(null);
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
}
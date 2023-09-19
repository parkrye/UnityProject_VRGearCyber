using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGR;
using UnityEngine.XR.Interaction.Toolkit;

namespace PID
{
    public class RobotCustomItemSocketInteractor : XRSocketInteractor
    {
        MeshRenderer _renderer;
        CustomGrabInteractable itemInteractable;
        [SerializeField] Color exertedColor, insertedColor, originalColor; 
        [SerializeField] Transform hoveringItem;
        [SerializeField] Transform hoveringLoc;
        [SerializeField] Vector3 hoveringItemScale, itemOriginalScale; 
        bool itemTaken; 
        RandomItem item;
        const string playerTag = "Player"; 

        protected override void Awake()
        {
            base.Awake();
            itemTaken = false;
            _renderer = GetComponent<MeshRenderer>();
            originalColor = _renderer.material.GetColor("_EmissionColor"); 
            item = GameManager.Resource.Load<RandomItem>("Data/ItemList");
            int seed = item.GetSeed();
            itemOriginalScale = item.itemRandomScale(seed); 
            GameObject spawningItem = GameManager.Resource.Instantiate(item.GetItem(seed), hoveringLoc.position, hoveringLoc.rotation, transform);
            RegisterInteractableItem(); 
        }

        public void RegisterInteractableItem()
        {
            itemInteractable = GetComponentInChildren<CustomGrabInteractable>();
            hoveringItem = itemInteractable.gameObject.transform;
            hoveringItem.transform.parent = null; 
            if (hoveringItemScale.sqrMagnitude > itemOriginalScale.sqrMagnitude)
            {
                hoveringItemScale = itemOriginalScale; 
            }
            hoveringItem.localScale = hoveringItemScale; 
        }

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            if (args.interactableObject.transform.gameObject.tag == playerTag)
            {
                if (!itemTaken && hoveringItem != null)
                    HandInteraction(true); 
            }
            base.OnHoverEntered(args);
        }
        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            if (args.interactableObject.transform.gameObject.tag == playerTag)
            {
                if (!itemTaken && hoveringItem != null)
                    HandInteraction(false); 
            }
            base.OnHoverExited(args);
        }
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            if (args.interactableObject is CustomGrabInteractable)
            {
                itemTaken = false; 
            }
            base.OnSelectEntered(args);
        }
        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
            if (args.interactableObject is CustomGrabInteractable)
                itemTaken = true; 
            base.OnSelectExiting(args);
        }
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            if (itemTaken && hoveringItem != null && gameObject.activeSelf == true)
            {
                ItemExert(); 
            }
            base.OnSelectExited(args);
        }

        private void HandInteraction(bool entered)
        {
            if (itemTaken)
                return;

            hoveringItem.parent = null; 
            if (entered)
            {
                HandInsert(); 
                hoveringItem.localScale = itemOriginalScale;
            }
            else
            {
                HandExert(); 
                hoveringItem.localScale = hoveringItemScale;
            }
        }

        private void ReScaleItem()
        {
            hoveringItem.localScale = itemOriginalScale; 
        }

        private void HandInsert()
        {
            _renderer.material.SetColor("_EmissionColor", insertedColor);
        }
        private void HandExert()
        {
            _renderer.material.SetColor("_EmissionColor", originalColor);
        }
        private void ItemExert()
        {
            _renderer.material.SetColor("_EmissionColor", exertedColor);
            if (hoveringItem != null)
            {
                hoveringItem.parent = null;
                hoveringItem.localScale = itemOriginalScale;
                itemTaken = true;
                hoveringItem = null;
                return;
            }
            return;
        }
    }
}
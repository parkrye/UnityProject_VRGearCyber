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
        [SerializeField] Color exertedColor;
        [SerializeField] Color insertedColor; 
        [SerializeField] Transform hoveringItem;
        [SerializeField] Transform hoveringLoc;
        [SerializeField] Vector3 hoveringItemScale, itemOriginalScale; 
        [SerializeField] SocketHelper socketHelper;
        bool itemTaken; 
        RandomItem item; 

        protected override void Awake()
        {
            base.Awake();
            itemTaken = false;
            _renderer = GetComponent<MeshRenderer>();
            item = GameManager.Resource.Load<RandomItem>("Data/ItemList");
            int seed = item.GetSeed();
            itemOriginalScale = item.itemRandomScale(seed); 
            GameObject spawningItem = GameManager.Resource.Instantiate(item.GetItem(seed), hoveringLoc.position, hoveringLoc.rotation, transform);
            RegisterInteractableItem(); 

        }

        public void RegisterInteractableItem()
        {
            itemInteractable = GetComponentInChildren<CustomGrabInteractable>();
            itemInteractable.selectEntered.AddListener(SelectEnterResponse);
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
            //CustomDirectInteractor testForPlayer = args.interactorObject.transform.gameObject.GetComponent<CustomDirectInteractor>();
            //if (!testForPlayer)
            //{
            //    return; 
            //}
            //if (args.interactableObject is CustomDirectInteractor)
            //{
            //    Debug.Log("Hands Detected");

            //}
            Debug.Log("Hands Detected");
            hoveringItem.localScale = Vector3.one;
            base.OnHoverEntered(args);
        }
        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            hoveringItem.localScale = hoveringItemScale;
            base.OnHoverExited(args);
        }
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            CustomGrabInteractable testForSocket = args.interactableObject.transform.gameObject.GetComponent<CustomGrabInteractable>();
            if (!testForSocket)
            {
                return;
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
            Debug.Log("Check Socket Exit"); 
            base.OnSelectExited(args);
        }
        public void SelectEnterResponse(SelectEnterEventArgs args)
        {
            CustomGrabInteractable testForSocket = args.interactableObject.transform.gameObject.GetComponent<CustomGrabInteractable>();
            if (!testForSocket)
            {
                return; 
            }

            StartCoroutine(ItemExert()); 
        }
        public void SelectExitResponse(SelectExitEventArgs args)
        {
            
        }
        private void HandEntered()
        {
            if (itemTaken)
                return;
            ReScaleItem(); 
        }

        private void ReScaleItem()
        {
            hoveringItem.localScale = hoveringItemScale; 
        }
        IEnumerator ItemExert()
        {
            _renderer.material.SetColor("_EmissionColor", exertedColor);
            hoveringItem.localScale = Vector3.one;
            hoveringItem = null; 
            yield return null; 
        }
    }
}
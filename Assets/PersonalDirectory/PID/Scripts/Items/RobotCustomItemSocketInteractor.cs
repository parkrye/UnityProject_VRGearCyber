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
        [SerializeField] Transform hoveringItem;
        [SerializeField] Transform hoveringLoc;
        [SerializeField] Vector3 hoveringItemScale; 
        RandomItem item; 

        protected override void Awake()
        {
            base.Awake();
            _renderer = GetComponent<MeshRenderer>();
            item = GameManager.Resource.Load<RandomItem>("Data/ItemList");
            GameManager.Resource.Instantiate(item.GetRandomItem(), hoveringLoc.position, hoveringLoc.rotation, transform);
            RegisterInteractableItem(); 
        }

        public void RegisterInteractableItem()
        {
            itemInteractable = GetComponentInChildren<CustomGrabInteractable>();
            itemInteractable.selectEntered.AddListener(SelectEnterResponse);
            hoveringItem = itemInteractable.gameObject.transform;
            hoveringItem.localScale = hoveringItemScale; 
        }

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            //CustomDirectInteractor testForPlayer = args.interactorObject.transform.gameObject.GetComponent<CustomDirectInteractor>();
            //if (!testForPlayer)
            //{
            //    return; 
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
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
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
        IEnumerator ItemExert()
        {
            _renderer.material.SetColor("_EmissionColor", exertedColor);
            hoveringItem.localScale = Vector3.one; 
            yield return null; 
        }
    }
}
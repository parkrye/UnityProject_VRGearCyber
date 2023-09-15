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
            hoveringItem.localScale = Vector3.one;
            base.OnHoverEntered(args);
        }
        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            hoveringItem.localScale = hoveringItemScale;
            base.OnHoverExited(args);
        }
        public void SelectEnterResponse(SelectEnterEventArgs args)
        {
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
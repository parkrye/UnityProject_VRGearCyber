using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class CableController : MonoBehaviour
    {
        [SerializeField] CableObject cableObject;

        [SerializeField] bool isCableLoaded, isSelectClicked;
        [SerializeField] float cableShotPower;

        void OnEnable()
        {
            cableObject.transform.position = transform.position;
        }

        public void OnSelectEnterEvent(SelectEnterEventArgs args)
        {
            isSelectClicked = true;

            GameObject target = args.interactableObject.transform.gameObject;
            if (!target || !target.GetComponent<CableObject>() || isCableLoaded)
                return;

            isCableLoaded = true;
        }

        public void OnSelectExitEvent(SelectExitEventArgs args)
        {
            isSelectClicked = false;

            if (!isCableLoaded)
                return;

            isCableLoaded = false;
        }

        public void OnLeftPrimaryButton(InputValue inputValue)
        {
            if (!isCableLoaded)
            {
                cableObject.transform.position = transform.position;
                return;
            }
            if (!isSelectClicked)
                return;

            cableObject.ShotCable(transform.forward, cableShotPower);
        }
    }
}
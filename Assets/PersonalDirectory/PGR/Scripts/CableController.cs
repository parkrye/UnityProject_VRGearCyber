using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class CableController : XRSocketInteractor
    {
        [Header("Cable Controller Parameters")]
        [SerializeField] CableObject cableObject;

        [SerializeField] bool isCableLoaded, isSelectClicked;
        [SerializeField] float cableShotPower;

        protected override void OnEnable()
        {
            base.OnEnable();
            cableObject.transform.position = transform.position;
        }

        public void OnSelectEnterEvent(SelectEnterEventArgs args)
        {
            isSelectClicked = true;

            GameObject target = args.interactableObject.transform.gameObject;
            if (!target || !target.GetComponent<CableObject>() || isCableLoaded)
                return;

            isCableLoaded = true;
            cableObject.ReadyToShot();
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
                cableObject.FixExit();
                cableObject.transform.position = transform.position;
                return;
            }
            if (!isSelectClicked)
                return;

            isSelectClicked = false;
            isCableLoaded = false;

            cableObject.ShotCable(transform.forward, cableShotPower);
        }
    }
}
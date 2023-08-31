using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class CableController : XRExclusiveSocketInteractor
    {
        [Header("Cable Controller Parameters")]
        [SerializeField] CableObject cableObject;

        [SerializeField] bool isCableLoaded, isSelectClicked;
        [SerializeField] float cableShotPower;

        protected override void OnEnable()
        {
            base.OnEnable();
            StartCoroutine(EnableRoutine());
        }

        IEnumerator EnableRoutine()
        {
            yield return new WaitForSeconds(1f);
            cableObject.transform.position = transform.position;
        }

        public void OnSelectEnteredEvent(SelectEnterEventArgs args)
        {
            isSelectClicked = true;

            GameObject target = args.interactableObject.transform.gameObject;
            if (!target || !target.GetComponent<CableObject>() || isCableLoaded)
                return;

            isCableLoaded = true;
            cableObject.ReadyToShot();
        }

        public void OnSelectExitedEvent(SelectExitEventArgs args)
        {
            isSelectClicked = false;
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
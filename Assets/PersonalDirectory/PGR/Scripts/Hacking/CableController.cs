using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class CableController : XRExclusiveSocketInteractor
    {
        [Header("Cable Controller Parameters")]
        [SerializeField] CableObject cableObject;

        [SerializeField] bool isCableLoaded, isSelectClicked;
        [SerializeField] float cableShotPower;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(WaitAwakeRoutine());
        }

        IEnumerator WaitAwakeRoutine()
        {
            yield return new WaitUntil(() => GameManager.Data != null);
            yield return new WaitUntil(() => GameManager.Data.Player != null);
            yield return new WaitUntil(() => GameManager.Data.Player.ExtraInput != null);
            GameManager.Data.Player.ExtraInput.LeftHandPrimaryButtonEvent.AddListener(CableAction);
        }

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

        public void CableAction(bool isPressed)
        {
            if (!isPressed)
                return;

            if (cableObject.State)
            {
                cableObject.ExitPuzzle();
            }

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
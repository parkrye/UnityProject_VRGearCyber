using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static PGR.CableObject;

namespace PGR
{
    public class CableController : XRExclusiveSocketInteractor
    {
        [Header("Cable Controller Parameters")]
        [SerializeField] CableObject cableObject;

        [SerializeField] float cableShotPower;
        [SerializeField] AudioSource shotAudio;

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

        public void CableAction(bool isPressed)
        {
            if (!isPressed)
                return;

            if (GameManager.Data.Player.HandMotion.IsAnyHandMotion(false))
                return;

            switch (cableObject.State)
            {
                case CableState.Hide:
                    cableObject.ReadyToShot();
                    break;
                case CableState.Ready:
                    StartCoroutine(ShotRoutine());
                    break;
                case CableState.Shot:
                    cableObject.ReturnToHand();
                    break;
                case CableState.Hack:
                    cableObject.ExitPuzzle();
                    break;
            }
        }

        IEnumerator ShotRoutine()
        {
            ChangeSocketType();

            shotAudio.Play();
            cableObject.ShotCable(transform.forward, cableShotPower);
            yield return new WaitForSeconds(1f);
            ChangeSocketType(GameData.InteractableType.Cable);
        }
    }
}
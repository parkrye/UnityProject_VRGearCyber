using PGR;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using static PID.Extension; 

namespace PID
{
    public class HealthSupplement : CustomGrabInteractable
    {

        CustomDirectInteractor playerHands;
        PlayerDataModel playerHealth;
        Renderer healthRender; 
        bool isActivated = false;
        bool isUsed = false; 
        HeldType heldType = HeldType.None;
        [SerializeField] int healthAmount; 
        [SerializeField] Color originalColor, usingColor, usedColor;
        [SerializeField] Vector3 holdOffset; 
        ActionBasedController playerHand; 
        protected override void Awake()
        {
            base.Awake();
            healthRender = GetComponent<Renderer>();
            originalColor = healthRender.material.GetColor("_EmissionColor"); 
        }
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            if (args.interactorObject is RobotCustomItemSocketInteractor)
            {
                base.OnSelectEntered(args);
                return;
            }
            //Set the obj a bit above the interactor obj. 
            //Get Access to Player Health; 
            playerHealth = GameManager.Data.Player.Data; 
        }

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            base.OnHoverEntered(args);
            if (!isActivated || isUsed)
            {
                return; 
            }
        }

        protected override void OnActivated(ActivateEventArgs args)
        {
            base.OnActivated(args);
            if (!isUsed)
            {
                playerHand = args.interactorObject.transform.GetComponentInParent<ActionBasedController>();
                StartCoroutine(GiveHealth(playerHand));
                return; 
            }
            isActivated = true;
        }
        protected override void OnDeactivated(DeactivateEventArgs args)
        {
            
            base.OnDeactivated(args);
            if (!isUsed)
            {
                StopAllCoroutines(); 
                healthRender.material.SetColor("_EmissionColor", originalColor);
            }
            isActivated = false;
        }
        const float usingTimeinterval = 1f;
        float timer;
        float percentage;
        IEnumerator GiveHealth(ActionBasedController controller = null)
        {
            timer = 0f; 
            while (timer < usingTimeinterval)
            {
                timer += Time.deltaTime;
                percentage = timer / usingTimeinterval;
                healthRender.material.SetColor("_EmissionColor", Color.Lerp(originalColor, usingColor * 20, percentage));
                yield return null;
            }
            isUsed = true;
            healthRender.material.SetColor("_EmissionColor", usingColor *  20);
            controller?.SendHapticImpulse(5f, .3f);
            GameManager.Data.Player.Data.GiveHealth(healthAmount); 
            healthRender.material.SetColor("_EmissionColor", usedColor * 20);
        }
    }

}

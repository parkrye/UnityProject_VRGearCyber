using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.InputSystem.InputAction;

public class InteractorController : MonoBehaviour
{
    [SerializeField] XRRayInteractor rayInteractor;
    [SerializeField] XRRayInteractor teleportInteractor;

    [SerializeField] List<LocomotionProvider> locomotions;

    [SerializeField] InputActionReference teleportModeActivate;

    private void Start()
    {
        if (rayInteractor != null)
        {
            rayInteractor.gameObject.SetActive(true);
        }
        if (teleportInteractor != null)
        {
            teleportInteractor.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (rayInteractor != null)
        {
            rayInteractor.selectEntered.AddListener(DisableLocomotions);
            rayInteractor.selectExited.AddListener(EnableLocomotions);
        }

        InputAction teleportModeActivate = this.teleportModeActivate?.action;
        if (teleportModeActivate != null)
        {
            teleportModeActivate.performed += OnStartTeleport;
            teleportModeActivate.canceled += OnStopTeleport;
        }
    }

    private void OnDisable()
    {
        if (rayInteractor != null)
        {
            rayInteractor.selectEntered.RemoveListener(DisableLocomotions);
            rayInteractor.selectExited.RemoveListener(EnableLocomotions);
        }

        InputAction teleportModeActivate = this.teleportModeActivate?.action;
        if (teleportModeActivate != null)
        {
            teleportModeActivate.performed -= OnStartTeleport;
            teleportModeActivate.canceled -= OnStopTeleport;
        }
    }

    private void EnableLocomotions(SelectExitEventArgs args)
    {
        foreach (LocomotionProvider locomotion in locomotions)
        {
            locomotion.gameObject.SetActive(true);
        }

        InputAction teleportModeActivate = this.teleportModeActivate?.action;
        if (teleportModeActivate != null)
            teleportModeActivate.Disable();
    }

    private void DisableLocomotions(SelectEnterEventArgs args)
    {
        foreach (LocomotionProvider locomotion in locomotions)
        {
            locomotion.gameObject.SetActive(false);
        }

        InputAction teleportModeActivate = this.teleportModeActivate?.action;
        if (teleportModeActivate != null)
            teleportModeActivate.Enable();
    }

    private void OnStartTeleport(CallbackContext context)
    {
        if (rayInteractor != null)
            rayInteractor.gameObject.SetActive(false);
        if (teleportInteractor != null)
            teleportInteractor.gameObject.SetActive(true);
    }

    private void OnStopTeleport(CallbackContext context)
    {
        if (rayInteractor != null)
            rayInteractor.gameObject.SetActive(true);
        if (teleportInteractor != null)
            StartCoroutine(TeleportDelay());
    }

    IEnumerator TeleportDelay()
    {
        yield return new WaitForEndOfFrame();   // Delay
        teleportInteractor.gameObject.SetActive(false);
    }
}

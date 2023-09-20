using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using PGR;
using System.Net.Sockets;

namespace KSI
{
	[RequireComponent(typeof(AudioSource))]
	public class CrossbowController : MonoBehaviour
    {
		[Header("Arrow")]
		[SerializeField] private float fireRate;
		[SerializeField] private GameObject arrow;
		[SerializeField] private Transform muzzlePoint;
		[SerializeField] private int damage;
		[SerializeField] private float maxDistance = 10;
		[SerializeField] float force;

		[Header("ArrowLocation")]
		[SerializeField] private ArrowLocation arrowLocation;
		[SerializeField] private XRBaseInteractor socketInteractor;
		[SerializeField] private XRExclusiveSocketInteractor socket;

		[Header("Audio")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip shootSound;
		[SerializeField] private AudioClip reload;
		[SerializeField] private AudioClip noAmmo;

		private Animator animator;
		private PlayerHandMotion playerHandMotion;
		private CustomDirectInteractor hand;
		private MeshRenderer muzzleFlash;
		private bool hasArrow = false;

		private void Start()
		{
			if (animator == null)
				animator = GetComponentInChildren<Animator>();

			muzzleFlash = muzzlePoint.GetComponentInChildren<MeshRenderer>();
			muzzleFlash.enabled = false;
		}

		public void AddArrow(SelectEnterEventArgs args)
		{
			if (!hasArrow)
			{
				hasArrow = true;

				audioSource.PlayOneShot(reload);

				arrowLocation = args.interactableObject.transform.GetComponent<ArrowLocation>();
				CustomGrabInteractable arrowLocationGrabInteractable = arrowLocation.GetComponent<CustomGrabInteractable>();
				XRGrabInteractable crossbowGrabInteractable = GetComponent<XRGrabInteractable>();

				foreach (Collider crossbowCollider in crossbowGrabInteractable.colliders)
				{
					foreach (Collider arrowLocationCollider in arrowLocationGrabInteractable.colliders)
					{
						Physics.IgnoreCollision(crossbowCollider, arrowLocationCollider, true);
					}
				}

				arrow = args.interactableObject.transform.gameObject;
			}
		}

		public void RemoveArrow(SelectExitEventArgs args)
		{
			hasArrow = false;

			audioSource.PlayOneShot(reload);

			XRGrabInteractable arrowLocationGrabInteractable = arrowLocation.GetComponent<XRGrabInteractable>();
			XRGrabInteractable crossbowGrabInteractable = GetComponent<XRGrabInteractable>();

			foreach (Collider crossbowCollider in crossbowGrabInteractable.colliders)
			{
				foreach (Collider arrowLocationCollider in arrowLocationGrabInteractable.colliders)
				{
					Physics.IgnoreCollision(crossbowCollider, arrowLocationCollider, false);
				}
			}

			arrow = null;
		}

		public void GrabCrossbow(SelectEnterEventArgs args)
		{
			hand = args.interactorObject.transform.GetComponent<CustomDirectInteractor>();
			if (hand == null)
				return;

			if (playerHandMotion == null)
				playerHandMotion = GameManager.Data.Player.HandMotion;

			if (hand.IsRightHand)
			{
				playerHandMotion.GrabOnGunRight(true);
			}
			else
			{
				playerHandMotion.GrabOnGunLeft(true);
			}
		}

		public void PutDownCrossbow(SelectExitEventArgs args)
		{
			hand = args.interactorObject.transform.GetComponent<CustomDirectInteractor>();
			if (hand == null)
				return;

			if (playerHandMotion == null)
				playerHandMotion = GameManager.Data.Player.HandMotion;

			if (hand.IsRightHand)
			{
				playerHandMotion.GrabOnGunRight(false);
			}
			else
			{
				playerHandMotion.GrabOnGunLeft(false);
			}
		}

		public void PullTheTrigger()
		{
			StartCoroutine(TriggerGunRoutine());

			if (hasArrow && arrow && arrowLocation.numberOfArrow > 0)
			{
				animator.SetTrigger("Fire");
				Shoot();
			}
			else
			{
				Debug.Log($"{gameObject.name} : No Arrow");

				audioSource.PlayOneShot(noAmmo);
			}
		}

		IEnumerator TriggerGunRoutine()
		{
			if (playerHandMotion == null)
				playerHandMotion = GameManager.Data.Player.HandMotion;

			if (hand.IsRightHand)
			{
				playerHandMotion.TriggerGunRight(true);
			}
			else
			{
				playerHandMotion.TriggerGunLeft(true);
			}

			yield return new WaitForSeconds(0.2f);

			if (hand.IsRightHand)
			{
				playerHandMotion.TriggerGunRight(false);
			}
			else
			{
				playerHandMotion.TriggerGunLeft(false);
			}
		}

		private void Shoot()
		{
			if (arrow == null)
				return;
		
			arrowLocation.numberOfArrow--;
			Debug.Log("Arrow used. Remaining bullets : " + arrowLocation.numberOfArrow);
			socket.ChangeSocketType();

			StartCoroutine(FireRoutine());			
		}

		private IEnumerator FireRoutine()
		{
			arrow.GetComponent<ArrowController>().FireArrow(force, damage);
			audioSource.PlayOneShot(shootSound, 1.0f);

			yield return new WaitForSeconds(1f);

			socket.ChangeSocketType(GameData.InteractableType.ArrowHole);
		}
	}
} 

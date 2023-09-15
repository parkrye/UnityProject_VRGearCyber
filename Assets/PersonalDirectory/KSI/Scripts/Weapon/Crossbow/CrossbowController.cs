using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using PGR;

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

		[Header("ArrowLocation")]
		public ArrowLocation arrowLocation;
		public XRBaseInteractor socketInteractor;

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
			if (hand == null)
				return;

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
			if (hasArrow && arrow && arrowLocation.numberOfArrow > 0)
			{
				animator.SetTrigger("Fire");
				Shoot();

				if (Physics.Raycast(muzzlePoint.position, muzzlePoint.forward, out RaycastHit hit, maxDistance))
				{
					Debug.Log($"Hit={hit.transform.name}");

					IHitable hitable = hit.transform.GetComponent<IHitable>();
					hitable?.TakeDamage(damage, hit.point, hit.normal);
				}
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
			arrowLocation.numberOfArrow--;
			Debug.Log("Arrow used. Remaining bullets : " + arrowLocation.numberOfArrow);
			Instantiate(arrow, muzzlePoint.position, muzzlePoint.rotation);
			audioSource.PlayOneShot(shootSound, 1.0f);
			StartCoroutine(MuzzleFlashRoutine());
		}

		IEnumerator MuzzleFlashRoutine()
		{
			Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
			muzzleFlash.material.mainTextureOffset = offset;
			float angle = Random.Range(0, 360);
			muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);
			float scale = Random.Range(1.0f, 2.0f);
			muzzleFlash.transform.localScale = Vector3.one * scale;
			muzzleFlash.enabled = true;
			yield return new WaitForSeconds(0.2f);
			muzzleFlash.enabled = false;
		}	
	}
} 

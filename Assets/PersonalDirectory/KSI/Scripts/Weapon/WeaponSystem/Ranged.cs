using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace KSI
{
	public class Ranged : MonoBehaviour
	{
		[Header("Settings")]
		[SerializeField] private float fireRate;
		[SerializeField] private GameObject bullet;
		[SerializeField] private Transform muzzlePoint;
		[SerializeField] private int damage;

		//[Header("Audio")]
		//[SerializeField] private AudioSource audioSource;
		//[SerializeField] private AudioClip shootSound;
		//[SerializeField] private AudioClip reload;
		//[SerializeField] private AudioClip noAmmo;

		[Header("Magazine")]
		public Magazine magazine;
		public XRBaseInteractor socketInteractor;

		private Animator animator;
		private bool hasSlide = false;

		private RaycastHit hit;
		private int layerMask;
		private Hitable hitable;

		public void AddMagazine(SelectEnterEventArgs args)
		{
			hasSlide = false;

			magazine = args.interactableObject.transform.GetComponent<Magazine>();
			XRGrabInteractable magazineGrabInteractable = magazine.GetComponent<XRGrabInteractable>();
			XRGrabInteractable pistolGrabInteractable = GetComponent<XRGrabInteractable>();
			foreach (Collider pistolCollider in pistolGrabInteractable.colliders)
			{
				foreach (Collider magaznineCollider in magazineGrabInteractable.colliders)
				{
					Physics.IgnoreCollision(pistolCollider, magaznineCollider, true);
				}
			}
		}

		public void RemoveMagazine(SelectExitEventArgs args)
		{
			magazine = null;
			//audioSource.PlayOneShot(reload);

			XRGrabInteractable magazineGrabInteractable = args.interactableObject.transform.GetComponent<XRGrabInteractable>();
			XRGrabInteractable pistolGrabInteractable = GetComponent<XRGrabInteractable>();
			foreach (Collider pistolCollider in pistolGrabInteractable.colliders)
			{
				foreach (Collider magaznineCollider in magazineGrabInteractable.colliders)
				{
					Physics.IgnoreCollision(pistolCollider, magaznineCollider, false);
				}
			}
		}

		public void Slide()
		{
			Debug.Log("Slided");
			hasSlide = true;
			//audioSource.PlayOneShot(reload);
		}

		private void Start()
		{
			animator = GetComponent<Animator>();

			// Enemy 레이어 마스크
			layerMask = (1 << 11) | (1 << 12);  

			if (socketInteractor != null)
			{
				socketInteractor.selectEntered.AddListener(AddMagazine);
				socketInteractor.selectExited.AddListener(RemoveMagazine);
			}
			else
			{
				Debug.LogError($"{gameObject.name} : socketInteractor is not set.");
			}
		}

		public void PullTheTrigger()
		{
			if (magazine && magazine.numberOfBullet > 0 && hasSlide)
			{
				// Ray 표시
				Debug.DrawRay(muzzlePoint.position, muzzlePoint.forward * 10.0f, Color.red);
				
				animator.SetTrigger("Fire");
				Shoot();

				// Ray 쏘기 
				if (Physics.Raycast(muzzlePoint.position, muzzlePoint.forward, out hit, 10.0f, layerMask))
				{
					Debug.Log($"Hit={hit.transform.name}");

					hitable = hit.transform.GetComponent<Hitable>();
					if (hitable != null)
					{
						StartCoroutine(hitable.Hit(damage));
					}
				}
			}
			else if (!hasSlide)
			{
				Debug.Log("No Slide");
			}
			else
			{
				Debug.Log($"{gameObject.name} : No Ammo");

				//audioSource.PlayOneShot(noAmmo);
			}			
		}

		public void Shoot()
		{
			magazine.numberOfBullet--;

			//audioSource.PlayOneShot(shootSound);

			Instantiate(bullet, muzzlePoint.position, muzzlePoint.rotation);
		}
	}
} 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using PGR;

namespace KSI
{
	[RequireComponent(typeof(AudioSource))]
	public class Ranged : MonoBehaviour
	{
		[Header("Gun")]
		[SerializeField] private float fireRate;
		[SerializeField] private GameObject bullet;
		[SerializeField] private Transform muzzlePoint;
		[SerializeField] private ParticleSystem bulletShell;
		[SerializeField] private int damage;
		[SerializeField] private float maxDistance = 10;

		[Header("Magazine")]
		public Magazine magazine;
		public XRBaseInteractor socketInteractor;

		[Header("Audio")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip shootSound;
		[SerializeField] private AudioClip reload;
		[SerializeField] private AudioClip noAmmo;

		private Animator animator;
		private PlayerHandMotion playerHandMotion;
		private bool isRightHanded;
		private MeshRenderer muzzleFlash;
		private bool hasMagazine = false;
		//private bool hasSlide = false;

		private void Start()
		{
			if (animator == null)
				animator = GetComponentInChildren<Animator>();

			// muzzlePoint 하위에 있는 muzzleFlashdml 컴포넌트 추출
			muzzleFlash = muzzlePoint.GetComponentInChildren<MeshRenderer>();
			muzzleFlash.enabled = false;

			if (playerHandMotion == null)
				playerHandMotion = GetComponent<PlayerHandMotion>();

			//if (socketInteractor != null)
			//{
			//	socketInteractor.selectEntered.AddListener(AddMagazine);
			//	socketInteractor.selectExited.AddListener(RemoveMagazine);
			//}
			//else
			//{
			//	Debug.LogError($"{gameObject.name} : socketInteractor is not set.");
			//}
		}

		public void AddMagazine(SelectEnterEventArgs args)
		{
			if (!hasMagazine)
			{
				hasMagazine = true;
				//hasSlide = false;

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
		}

		public void RemoveMagazine(SelectExitEventArgs args)
		{
			hasMagazine = false;

			audioSource.PlayOneShot(reload);

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

		//public void Slide()
		//{
		//	Debug.Log("Slided");
		//	hasSlide = true;
		//	//audioSource.PlayOneShot(reload);
		//}

		public void PullTheTrigger(ActivateEventArgs args)
		{
			if (hasMagazine && magazine && magazine.numberOfBullet > 0) //&& hasSlide)
			{
				if (playerHandMotion == null)
					playerHandMotion = GameManager.Data.Player.HandMotion;
				if (isRightHanded)
				{
					playerHandMotion.TriggerGunRight(true);
				}
				else
				{
					playerHandMotion.TriggerGunLeft(true);
				}

				animator.SetTrigger("Fire");
				Shoot();

				if (Physics.Raycast(muzzlePoint.position, muzzlePoint.forward, out RaycastHit hit, maxDistance))
				{
					Debug.Log($"Hit={hit.transform.name}");

					IHitable hitable = hit.transform.GetComponent<IHitable>();
					hitable?.TakeDamage(damage, hit.point, hit.normal);
				}				
			}
			//else if (!hasSlide)
			//{
			//	Debug.Log("No Slide");
			//}
			else
			{
				Debug.Log($"{gameObject.name} : No Ammo");

				audioSource.PlayOneShot(noAmmo);
			}
		}
			
		private void Shoot()
		{
			magazine.numberOfBullet--;

			Instantiate(bullet, muzzlePoint.position, muzzlePoint.rotation);
			audioSource.PlayOneShot(shootSound, 1.0f);
			bulletShell.Play();

			StartCoroutine(MuzzleFlashRoutine());
		}

		IEnumerator MuzzleFlashRoutine()
		{
			// 오프셋 좌푯값을 랜덤 함수로 생성
			Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
			// 텍스처의 오프셋 값 설정
			muzzleFlash.material.mainTextureOffset = offset;

			// MuzzleFlash의 회전 변경
			float angle = Random.Range(0, 360);
			muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);

			// MuzzleFlash의 크기 조절
			float scale = Random.Range(1.0f, 2.0f);
			muzzleFlash.transform.localScale = Vector3.one * scale;

			muzzleFlash.enabled = true;

			yield return new WaitForSeconds(0.2f);

			muzzleFlash.enabled = false;
			if (isRightHanded)
			{
				playerHandMotion.TriggerGunRight(false);
			}
			else
			{
				playerHandMotion.TriggerGunLeft(false);
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawRay(muzzlePoint.position, muzzlePoint.forward * maxDistance);
		}
	}
} 
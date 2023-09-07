using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace KSI
{
	[RequireComponent(typeof(AudioSource))]
	public class Ranged : MonoBehaviour
	{
		[Header("Buleet")]
		[SerializeField] private float fireRate;
		[SerializeField] private GameObject bullet;
		//[SerializeField] private GameObject casingPrefab;
		[SerializeField] private Transform muzzlePoint;
		//[SerializeField] private Transform casingExitLocation;
		[SerializeField] private int damage;
		
		[Header("Magazine")]
		public Magazine magazine;
		public XRBaseInteractor socketInteractor;

		[Header("Audio")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip shootSound;
		[SerializeField] private AudioClip reload;
		[SerializeField] private AudioClip noAmmo;

		//[Header("Settings")]
		//[SerializeField] private float destroyTimer = 2f;
		//[SerializeField] private float shotPower = 500f;
		//[SerializeField] private float ejectPower = 150f;

		private Animator animator;
		private MeshRenderer muzzleFlash;
		private bool hasMagazine = false;
		//private bool hasSlide = false;

		//private RaycastHit hit;
		//private int layerMask;
		//private Hitable hitable;

		private void Start()
		{
			if (animator == null)
				animator = GetComponentInChildren<Animator>();

			// muzzlePoint 하위에 있는 muzzleFlashdml 컴포넌트 추출
			muzzleFlash = muzzlePoint.GetComponentInChildren<MeshRenderer>();
			muzzleFlash.enabled = false;

			// Enemy 레이어 마스크
			//layerMask = (1 << 11) | (1 << 12);  

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

		public void PullTheTrigger()
		{
			if (hasMagazine && magazine && magazine.numberOfBullet > 0) //&& hasSlide)
			{
				// Ray 표시
				//Debug.DrawRay(muzzlePoint.position, muzzlePoint.forward * 10.0f, Color.red);
				
				animator.SetTrigger("Fire");
				Shoot();

				// Ray 쏘기 
				//if (Physics.Raycast(muzzlePoint.position, muzzlePoint.forward, out hit, 10.0f, layerMask))
				//{
				//	Debug.Log($"Hit={hit.transform.name}");

				//	hitable = hit.transform.GetComponent<Hitable>();
				//	if (hitable != null)
				//	{
				//		StartCoroutine(hitable.Hit(damage));
				//	}
				//}
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
		}

		private void CasingRelease()
		{			
			//if (!casingExitLocation || !casingPrefab)
			//	return;

			//GameObject tempCasing;
			//tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
			//tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
			//tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

			//Destroy(tempCasing, destroyTimer);
		}
	}
} 
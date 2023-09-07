using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace KSI
{
    public class CrossbowController : MonoBehaviour
    {
		public float shotPower = 100f;

		[Header("Arrow")]
		[SerializeField] private float fireRate;
		[SerializeField] private GameObject arrow;
		[SerializeField] private Transform muzzlePoint;
		[SerializeField] private int damage;

		[Header("ArrowLocation")]
		public ArrowLocation arrowLocation;
		public XRBaseInteractor socketInteractor;

		[Header("Audio")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip shootSound;
		[SerializeField] private AudioClip reload;
		[SerializeField] private AudioClip noAmmo;

		private Animator animator;
		private MeshRenderer muzzleFlash;
		private bool hasArrow = false;

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

		public void AddArrow(SelectEnterEventArgs args)
		{
			if (!hasArrow)
			{
				hasArrow = true;

				arrowLocation = args.interactableObject.transform.GetComponent<ArrowLocation>();
				XRGrabInteractable arrowLocationGrabInteractable = arrowLocation.GetComponent<XRGrabInteractable>();
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


		public void PullTheTrigger()
		{
			if (hasArrow && arrow && arrowLocation.numberOfArrow > 0)
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
			else
			{
				Debug.Log($"{gameObject.name} : No Arrow");

				audioSource.PlayOneShot(noAmmo);
			}
		}

		private void Shoot()
		{
			arrowLocation.numberOfArrow--;

			Instantiate(arrow, muzzlePoint.position, muzzlePoint.rotation);
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
	}
} 

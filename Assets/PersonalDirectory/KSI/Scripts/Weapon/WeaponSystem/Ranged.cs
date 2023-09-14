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

		[Header("Enemy")]
		[SerializeField] float soundIntensity;

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
		private CustomDirectInteractor hand;
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
			if (hand == null)
				return;

			if (!hasMagazine)
			{
				hasMagazine = true;
				//hasSlide = false;

				magazine = args.interactableObject.transform.GetComponent<Magazine>();
				CustomGrabInteractable magazineGrabInteractable = magazine.GetComponent<CustomGrabInteractable>();
				XRGrabInteractable pistolGrabInteractable = GetComponent<XRGrabInteractable>();

				foreach (Collider pistolCollider in pistolGrabInteractable.colliders)
				{
					foreach (Collider magaznineCollider in magazineGrabInteractable.colliders)
					{
						Physics.IgnoreCollision(pistolCollider, magaznineCollider, true);
					}
				}

				if(hand.IsRightHand)
					GameManager.Data.Player.ExtraInput.RightHandPrimaryButtonEvent.AddListener(EjectMagazine);
				else
					GameManager.Data.Player.ExtraInput.LeftHandPrimaryButtonEvent.AddListener(EjectMagazine);
					magazineGrabInteractable.InteractableType = GameData.InteractableType.None;
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

		private void EjectMagazine(bool isPressed)
		{
			if (magazine != null && isPressed)
			{
				magazine.transform.SetParent(null);
				magazine = null;
			}
		}

		//public void Slide()
		//{
		//	Debug.Log("Slided");
		//	hasSlide = true;
		//	//audioSource.PlayOneShot(reload);
		//}
		
		public void MakeSound(Vector3 soundPos, Vector3 hitPoint) 
		{
			Collider[] soundPoints = Physics.OverlapSphere(soundPos, soundIntensity); 
			if (soundPoints.Length <= 0 ) {
				return; 
			}
			foreach (Collider soundPoint in soundPoints) 
			{
				PID.IHearable soundhearer = soundPoint.gameObject.GetComponent<PID.IHearable>();
				soundhearer?.Heard(soundPos, GameManager.traceSound.WallIntersect(soundPos, hitPoint));
			}
		}

		public void GrabGun(SelectEnterEventArgs args)
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

		public void PutDownGun(SelectExitEventArgs args)
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


		public void PullTheTrigger(ActivateEventArgs args)
		{
			StartCoroutine(TriggerGunRoutine());

			if (hasMagazine && magazine && magazine.numberOfBullet > 0) //&& hasSlide)
			{

				animator.SetTrigger("Fire");
				Shoot();

				if (Physics.Raycast(muzzlePoint.position, muzzlePoint.forward, out RaycastHit hit, maxDistance))
				{
					Debug.Log($"Hit={hit.transform.name}");

					//IHitable hitable = hit.transform.GetComponent<IHitable>();
					//hitable?.TakeDamage(damage, hit.point, hit.normal);

					IHitable hitable = hit.transform.GetComponent<IHitable>();
					if (hitable != null)
					{
						Debug.Log($"Hitable Object: {hitable.ToString()}");

						hitable.TakeDamage(damage, hit.point, hit.normal);
					}
					else
					{
						Debug.Log("No hitable component found on hit object.");
					}
				}
				//Make sound for non hits 
				//Vector3 virtualSoundPoint = Vector3.Dot(muzzlePoint.position, muzzlePoint.forward)
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

			yield return new WaitForSeconds(1f);

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
			magazine.numberOfBullet--;
			Debug.Log("Bullet used. Remaining bullets: " + magazine.numberOfBullet);

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
		}
	}
} 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using PGR;
using PID;

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

			// muzzlePoint ?òÏúÑ???àÎäî muzzleFlashdml Ïª¥Ìè¨?åÌä∏ Ï∂îÏ∂ú
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
				{
                    GameManager.Data.Player.ExtraInput.LeftHandPrimaryButtonEvent.AddListener(EjectMagazine);
                    magazineGrabInteractable.InteractableType = GameData.InteractableType.None;
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
						MakeSound(hit.point);
						hitable.TakeDamage(damage, hit.point, hit.normal);
					}
					else
					{
						MakeSound(hit.point);
					}
				}
				else 
				{
					if (Physics.Raycast(muzzlePoint.position, muzzlePoint.forward, out RaycastHit hit_2)) 
					{
						MakeSound(hit_2.point);
					}
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
		public void MakeSound(Vector3 pos)
		{
			Collider[] soundHits = Physics.OverlapSphere(pos, soundIntensity);
			if (soundHits.Length <= 0)
				return;
			foreach (Collider collider in soundHits)
			{
				IHearable hearable = collider.gameObject.GetComponent<IHearable>();
				hearable?.Heard(pos, GameManager.traceSound.WallIntersect(pos, collider.transform.position));
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
			magazine.numberOfBullet--;
			Debug.Log("Bullet used. Remaining bullets: " + magazine.numberOfBullet);

			Instantiate(bullet, muzzlePoint.position, muzzlePoint.rotation);
			audioSource.PlayOneShot(shootSound, 1.0f);
			bulletShell.Play();

			StartCoroutine(MuzzleFlashRoutine());

			//if (magazine.numberOfBullet > 0 && magazine.bullets.Count > 0)
			//{
			//	magazine.numberOfBullet--;

			//	GameObject bulletToRemove = magazine.bullets[0];
			//	magazine.bullets.RemoveAt(0);
			//	Destroy(bulletToRemove);

			//	Instantiate(bullet, muzzlePoint.position, muzzlePoint.rotation);
			//	audioSource.PlayOneShot(shootSound, 1.0f);
			//	bulletShell.Play();
			//	StartCoroutine(MuzzleFlashRoutine());

			//	Debug.Log("Bullet used. Remaining bullets: " + magazine.numberOfBullet);
			//}
			//else
			//{
			//	Debug.Log($"{gameObject.name} : No Ammo");
			//	audioSource.PlayOneShot(noAmmo);
			//}
		}

		IEnumerator MuzzleFlashRoutine()
		{
			// ?§ÌîÑ??Ï¢åÌëØÍ∞íÏùÑ ?úÎç§ ?®ÏàòÎ°??ùÏÑ±
			Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
			// ?çÏä§Ï≤òÏùò ?§ÌîÑ??Í∞??§Ï†ï
			muzzleFlash.material.mainTextureOffset = offset;

			// MuzzleFlash???åÏ†Ñ Î≥ÄÍ≤?
			float angle = Random.Range(0, 360);
			muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);

			// MuzzleFlash???¨Í∏∞ Ï°∞Ï†à
			float scale = Random.Range(1.0f, 2.0f);
			muzzleFlash.transform.localScale = Vector3.one * scale;

			muzzleFlash.enabled = true;

			yield return new WaitForSeconds(0.2f);

			muzzleFlash.enabled = false;
		}
	}
} 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using PGR;
using PID;
using TMPro;
using UnityEngine.UI;

namespace KSI
{
	[RequireComponent(typeof(AudioSource))]
	public class Ranged : MonoBehaviour
	{
		[Header("Gun")]
		[SerializeField] private float fireRate;
		[SerializeField] private GameObject bullet;
		[SerializeField] private Transform muzzlePoint;
		[SerializeField] private int damage;
		[SerializeField] private float maxDistance = 10;

		[Header("Enemy")]
		[SerializeField] float soundIntensity;

		[Header("Magazine")]
		public Magazine magazine;
		public XRBaseInteractor socketInteractor;
		public Image Image;
		public TextMeshProUGUI text;
	

		[Header("Audio")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip shootSound;
		[SerializeField] private AudioClip reload;
		[SerializeField] private AudioClip noAmmo;

		private Animator animator;
		
		private PlayerHandMotion playerHandMotion;
		private CustomDirectInteractor hand;
		private bool hasMagazine = false;
		private int remainBullet;


		private void Start()
		{
			if (animator == null)
				animator = GetComponentInChildren<Animator>();
		}

		public void AddMagazine(SelectEnterEventArgs args)
		{
			if (hand == null)
				return;

			if (!hasMagazine)
			{
				hasMagazine = true;

				audioSource.PlayOneShot(reload);

				magazine = args.interactableObject.transform.GetComponent<Magazine>();
				CustomGrabInteractable magazineGrabInteractable = magazine.GetComponent<CustomGrabInteractable>();
				XRGrabInteractable gunGrabInteractable = GetComponent<XRGrabInteractable>();

				foreach (Collider gunCollider in gunGrabInteractable.colliders)
				{
					foreach (Collider magaznineCollider in magazineGrabInteractable.colliders)
					{
						Physics.IgnoreCollision(gunCollider, magaznineCollider, true);
					}
				}

				if(hand.IsRightHand)
					GameManager.Data.Player.ExtraInput.RightHandPrimaryButtonEvent.AddListener(EjectMagazine);
				else
				{
                    GameManager.Data.Player.ExtraInput.LeftHandPrimaryButtonEvent.AddListener(EjectMagazine);                
                }
			}

			Image.fillAmount = 1.0f;
			remainBullet = magazine.numberOfBullet;
			UpdateBulletText();
		}

		public void RemoveMagazine(SelectExitEventArgs args)
		{
			hasMagazine = false;

			audioSource.PlayOneShot(reload);

			XRGrabInteractable magazineGrabInteractable = args.interactableObject.transform.GetComponent<XRGrabInteractable>();
			XRGrabInteractable gunGrabInteractable = GetComponent<XRGrabInteractable>();
			foreach (Collider gunCollider in gunGrabInteractable.colliders)
			{
				foreach (Collider magaznineCollider in magazineGrabInteractable.colliders)
				{
					Physics.IgnoreCollision(gunCollider, magaznineCollider, false);
				}
			}
		}

		private void EjectMagazine(bool isPressed)
		{
			if (magazine != null && isPressed)
			{
				magazine.GetComponent<CustomGrabInteractable>().InteractableType = GameData.InteractableType.None;
				Debug.Log("EjectMagazine");
				magazine.transform.SetParent(null);
				magazine = null;
			}
		}

		public void GrabGun(SelectEnterEventArgs args)
		{
			if (args.interactorObject.transform.GetComponent<XRSocketInteractor>())
				return;
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
			if (args.interactorObject.transform.GetComponent<XRSocketInteractor>())
				return;
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
			if (hand == null)
				return;
			StartCoroutine(TriggerGunRoutine());

			if (hasMagazine && magazine && magazine.numberOfBullet > 0)
			{

				animator.SetTrigger("Fire");
				Shoot();

				if (Physics.Raycast(muzzlePoint.position, muzzlePoint.forward, out RaycastHit hit, maxDistance))
				{
					Debug.Log($"Hit={hit.transform.name}");	

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
			Image.fillAmount = (float)remainBullet / (float)magazine.numberOfBullet;
			UpdateBulletText();
			Debug.Log("Bullet used. Remaining bullets : " + magazine.numberOfBullet);
			Instantiate(bullet, muzzlePoint.position, muzzlePoint.rotation);
			audioSource.PlayOneShot(shootSound, 1.0f);
		}

		private void UpdateBulletText()
		{
			text.text = $"{remainBullet} / {magazine.numberOfBullet}";
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
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using PID;
using PGR;

namespace KSI
{
	public class Melee : MonoBehaviour
	{
		[Header("Melee")]
		[SerializeField] private float checkRate = 0.1f;
		[SerializeField] private int maxPositions = 10;
		[SerializeField] private float maxDamage = 100;
		[SerializeField] private float minDamage = 10;
		[SerializeField] private float minDistanceForMaxDamage = 1.0f;
		[SerializeField] private float minDistanceForMinDamage = 0.2f;
		[SerializeField] private Transform attackPoint;

		[Header("")]
		[SerializeField] private Collider coll;
		[SerializeField] private bool isSwinging = false;

		private bool IsSwinging { get { return isSwinging; } set { isSwinging = value; Debug.Log($"{name} is {value}"); } }
		private Queue<Vector3> positionQueue = new Queue<Vector3>();
		private PlayerHandMotion playerHandMotion;
		private bool isRightHanded;

		void Start()
		{
			if (playerHandMotion == null)
				playerHandMotion = GetComponent<PlayerHandMotion>();
		}

		public void StartSwing(SelectEnterEventArgs args)
		{
			if (args.interactorObject.transform.GetComponent<CustomDirectInteractor>() == null)
				return;

			if (playerHandMotion == null)
				playerHandMotion = GameManager.Data.Player.HandMotion;

			if (isRightHanded == true)
			{
				playerHandMotion.GrabOnCloseWeaponRight(true);
			}
			else
			{
				playerHandMotion.GrabOnCloseWeaponLeft(true);
			}

			StartCoroutine(TrackPositionRoutine());
			IsSwinging = true;
			coll.isTrigger = true;
		}

		private IEnumerator TrackPositionRoutine()
		{
			while (true)
			{
				if (isSwinging)
				{
					if (positionQueue.Count >= maxPositions)
					{
						positionQueue.Dequeue();
					}
					positionQueue.Enqueue(attackPoint.position);
				}

				yield return new WaitForSeconds(checkRate);
			}
		}

		public void EndSwing(SelectExitEventArgs args)
		{
			if (args.interactorObject.transform.GetComponent<CustomDirectInteractor>() == null)
				return;

			if (playerHandMotion == null)
				playerHandMotion = GameManager.Data.Player.HandMotion;

			if (isRightHanded == true)
			{
				playerHandMotion.GrabOnCloseWeaponRight(false);
			}
			else
			{
				playerHandMotion.GrabOnCloseWeaponLeft(false);
			}

			StopAllCoroutines();
			IsSwinging = false;
			coll.isTrigger = false;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (isSwinging && positionQueue.Count > 0)
			{
				IStrikable iStrikable = other.gameObject.GetComponent<IStrikable>();
				Debug.Log($"{iStrikable}");
				if (iStrikable != null)
				{
					Vector3 oldestPosition = positionQueue.Peek();
					Vector3 currentPosition = attackPoint.position;
					float distance = Vector3.Distance(oldestPosition, currentPosition);

					float calculateDamageRatio = Mathf.InverseLerp(minDistanceForMinDamage, minDistanceForMaxDamage, distance);
					int calculateDamage = Mathf.RoundToInt(Mathf.Lerp(minDamage, maxDamage, calculateDamageRatio));

					iStrikable?.TakeStrike(transform, calculateDamage, Vector3.zero, Vector3.zero);
					Debug.Log($"{calculateDamage}");
				}
			}
		}
	}
}
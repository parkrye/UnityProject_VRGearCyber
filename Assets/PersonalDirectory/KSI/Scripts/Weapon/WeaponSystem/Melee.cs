using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PID;
using PGR;
using UnityEngine.XR.Interaction.Toolkit;

namespace KSI
{
    public class Melee : MonoBehaviour
    {
		[Header("Settings")]
		[SerializeField] private float checkRate = 0.1f;
		[SerializeField] private int maxPositions = 10;
		[SerializeField] private float maxDamage = 100;
		[SerializeField] private float minDamage = 10;
		[SerializeField] private float minDistanceForMaxDamage = 1.0f;
		[SerializeField] private float minDistanceForMinDamage = 0.2f;
		
		[Header("Runtime Data")]
		[SerializeField] private Collider coll;

		private bool isRightHanded; 		
		[SerializeField] private bool isSwinging = false;
		bool IsSwinging{ get{ return isSwinging; } set { isSwinging = value; Debug.Log($"{name} is {value}"); } }
		private Queue<Vector3> positionQueue = new Queue<Vector3>();
		private PlayerHandMotion playerHandMotion;

		void Start()
		{
			if (playerHandMotion == null)
				playerHandMotion = GetComponent<PlayerHandMotion>();
		}

		IEnumerator TrackPositionRoutine()
		{
			while (true)
			{
				if (isSwinging)
				{
					if (positionQueue.Count >= maxPositions)
					{
						positionQueue.Dequeue(); 
					}
					positionQueue.Enqueue(transform.position);
				}

				yield return new WaitForSeconds(checkRate); 
			}
		}

		public void StartSwing(SelectEnterEventArgs args)
        {
			//Debug.Log("StartSwing");

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

		public void EndSwing(SelectExitEventArgs args)
		{
			//Debug.Log("EndSwing");

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
					Vector3 currentPosition = transform.position;
					float distance = Vector3.Distance(oldestPosition, currentPosition);

					float calculateDamageRatio = Mathf.InverseLerp(minDistanceForMinDamage, minDistanceForMaxDamage, distance);
					int calculateDamage = Mathf.RoundToInt(Mathf.Lerp(minDamage, maxDamage, calculateDamageRatio));

					iStrikable?.TakeStrike(transform, calculateDamage, Vector3.zero, Vector3.zero);
				}
			}
		}
	}
}
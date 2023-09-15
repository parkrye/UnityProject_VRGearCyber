using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PID;
using PGR;

namespace KSI
{
    public class Melee : MonoBehaviour
    {
		[Header("Settings")]
		[SerializeField] private float checkRate = 0.1f; // 위치를 체크하는 주기
		[SerializeField] private int maxPositions = 10; // 큐에 저장할 최대 위치 개수
		[SerializeField] private float maxDamage = 100; // 최대 데미지 값
		[SerializeField] private float minDamage = 10; // 최소 데미지 값
		[SerializeField] private float minDistanceForMaxDamage = 1.0f; // 최대 데미지를 주기 위한 최소 거리
		[SerializeField] private float minDistanceForMinDamage = 0.2f; // 최소 데미지를 주기 위한 최소 거리
		
		[Header("Runtime Data")]
		[SerializeField] private Collider coll; // 무기의 콜라이더

		private bool isRightHanded; 		
		[SerializeField] private bool isSwinging = false; // 무기가 휘두르고 있는지 여부
		bool IsSwinging{ get{ return isSwinging; } set { isSwinging = value; Debug.Log($"{name} is {value}"); } }
		private Queue<Vector3> positionQueue = new Queue<Vector3>(); // 위치를 저장할 큐		
		private PlayerHandMotion playerHandMotion;

		void Start()
		{
			if (playerHandMotion == null)
				playerHandMotion = GetComponent<PlayerHandMotion>();
		}

		// 위치를 추적하는 코루틴
		IEnumerator TrackPositionRoutine()
		{
			while (true)
			{
				if (isSwinging)
				{
					if (positionQueue.Count >= maxPositions)
					{
						// 큐가 가득 차면, 가장 오래된 위치 제거
						positionQueue.Dequeue(); 
					}
					// 현재 위치를 큐에 추가
					positionQueue.Enqueue(transform.position);
				}

				// 다음 체크까지 대기
				yield return new WaitForSeconds(checkRate); 
			}
		}

		public void StartSwing()
        {
			Debug.Log("StartSwing");

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
		}

		public void EndSwing()
		{
			Debug.Log("EndSwing");

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
		}

		private void OnCollisionEnter(Collision other)
		{
			Debug.Log("OnColliderEnter" + other.gameObject.name);

			Debug.Log($"{isSwinging}, {positionQueue.Count}");
			
			if (isSwinging && positionQueue.Count > 0)

			{
				IStrikable iStrikable = other.gameObject.GetComponent<IStrikable>();
				Debug.Log($"{iStrikable}");
				if (iStrikable != null)
				{
					// 가장 오래된 위치 가져오기
					Vector3 oldestPosition = positionQueue.Peek();
					// 현재 위치 가져오기
					Vector3 currentPosition = transform.position;
					// 두 위치 사이의 거리 계산
					float distance = Vector3.Distance(oldestPosition, currentPosition);

					// 데미지 비율 계산
					float calculateDamageRatio = Mathf.InverseLerp(minDistanceForMinDamage, minDistanceForMaxDamage, distance);
					// 실제 데미지 값 계산
					int calculateDamage = Mathf.RoundToInt(Mathf.Lerp(minDamage, maxDamage, calculateDamageRatio));

					// 계산된 데미지 적용
					iStrikable?.TakeStrike(transform, calculateDamage, Vector3.zero, Vector3.zero);
					Debug.Log("Calculated damage: " + calculateDamage);
				}
			}
		}
	}
}
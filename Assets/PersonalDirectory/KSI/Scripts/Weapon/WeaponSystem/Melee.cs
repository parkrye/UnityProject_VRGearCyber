using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		private bool isSwinging = false; // 무기가 휘두르고 있는지 여부
		private Queue<Vector3> positionQueue = new Queue<Vector3>(); // 위치를 저장할 큐

		void Start()
		{
			// 초기에는 콜라이더를 비활성화
			//coll.enabled = false;
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
			StartCoroutine(TrackPositionRoutine());

			isSwinging = true;
		}

		public void EndSwing()
		{
			StopAllCoroutines();

			isSwinging = false;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (isSwinging && positionQueue.Count > 0)
			{
				IHitable hitable = other.GetComponent<IHitable>(); 
				if (hitable != null)
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

					// 계신된 데미지 적용
					hitable.TakeDamage(calculateDamage, Vector3.zero, Vector3.zero);
				}
			}
		}
	}
}
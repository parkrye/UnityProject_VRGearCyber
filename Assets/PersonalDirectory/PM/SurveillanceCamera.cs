using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceCamera: MonoBehaviour
{
    [SerializeField]
    GameObject[] securities;
    [SerializeField] float range;
    [SerializeField, Range(0, 360)] float angle;
    IEnumerator Checking()
    {
        while(true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, range);
            foreach(Collider collider in colliders)
            {
                // 만약 콜라이더가 플레이어면 실행
                //if(collider.GetComponent<>() != null)
                Vector3 dirTarget = (collider.transform.position - transform.position).normalized;
                if (Vector3.Dot(transform.position, dirTarget) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad))
                    continue;
                foreach(GameObject guard in securities)
                    // 로봇의 위치를 추적하는 함수를 가져와서 플레이어의 위치를 변수로 값을 넘겨줌 guard.GetComponent<>
                yield return null;
            }
            yield return null;
        }
    }
}

using UnityEngine;

namespace PGR
{
    public class PlayerHandWallCheck : MonoBehaviour
    {
        [SerializeField] PlayerController playerController;
        [SerializeField] Transform handTransform, spineTransform;
        [SerializeField] LayerMask wallLayer;
        [SerializeField] bool isStop, isRight, isRayCasted;

        [SerializeField] Vector3 wallNormalVector, handLookPosition;
        [SerializeField] GameObject handPrefab;

        void LateUpdate()
        {
            if (!isStop)
                return;

            if (!isRayCasted)
                return;

            handLookPosition = Vector3.ProjectOnPlane((transform.position - handTransform.position), wallNormalVector).normalized;

            if(isRight)
                handTransform.rotation = Quaternion.Lerp(handTransform.rotation, Quaternion.LookRotation(handLookPosition, wallNormalVector), 0.1f);
            else
                handTransform.rotation = Quaternion.Lerp(handTransform.rotation, Quaternion.LookRotation(handLookPosition, -wallNormalVector), 0.1f);

            // 손이 벽에 닿았을때 움직이는 것 추가
            if (Physics.Raycast(spineTransform.position, transform.position - spineTransform.position, out RaycastHit hit, 3f, wallLayer))
            {
                handTransform.position = Vector3.Lerp(handTransform.position, hit.point, 0.1f);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if(!isStop && 1 << other.gameObject.layer == wallLayer)
            {
                isStop = true;
                playerController.HandMotion.WallCheck(isRight, isStop);

                if (Physics.Raycast(transform.position, other.transform.position - transform.position, out RaycastHit hit))
                {
                    isRayCasted = true;
                    wallNormalVector = hit.normal.normalized;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (1 << other.gameObject.layer == wallLayer)
            {
                isStop = false;
                isRayCasted = false;
                playerController.HandMotion.WallCheck(isRight, isStop);
                handTransform.localPosition = handPrefab.transform.localPosition;
                handTransform.localRotation = handPrefab.transform.localRotation;
            }
        }
    }

}
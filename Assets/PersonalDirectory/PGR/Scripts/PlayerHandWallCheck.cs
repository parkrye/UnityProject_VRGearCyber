using UnityEngine;

namespace PGR
{
    public class PlayerHandWallCheck : MonoBehaviour
    {
        [SerializeField] PlayerController playerController;
        [SerializeField] Transform handTransform;
        [SerializeField] Vector3 lastPosition;
        [SerializeField] LayerMask wallLayer;
        [SerializeField] bool isStop, isRight, isRayCasted;

        [SerializeField] Vector3 wallNormalVector, handLookPosition;

        void LateUpdate()
        {
            if (!isStop)
                return;

            handTransform.position = lastPosition;

            if (!isRayCasted)
                return;

            handLookPosition = lastPosition + Vector3.ProjectOnPlane((transform.position - lastPosition), wallNormalVector).normalized;
            //handTransform.LookAt(handLookPosition);
            handTransform.rotation = Quaternion.LookRotation(handLookPosition, lastPosition + wallNormalVector);
        }

        void OnTriggerEnter(Collider other)
        {
            if(!isStop && 1 << other.gameObject.layer == wallLayer)
            {
                lastPosition = transform.position;
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
            }
        }
    }

}
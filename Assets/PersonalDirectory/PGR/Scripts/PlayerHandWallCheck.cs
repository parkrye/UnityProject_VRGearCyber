using UnityEngine;

namespace PGR
{
    public class PlayerHandWallCheck : MonoBehaviour
    {
        [SerializeField] PlayerController playerController;
        [SerializeField] Transform handTransform;
        [SerializeField] Vector3 lastPosition;
        [SerializeField] LayerMask wallLayer;
        [SerializeField] bool isStop, isRight;

        void LateUpdate()
        {
            if (!isStop)
                return;

            handTransform.position = lastPosition;
        }

        void OnTriggerEnter(Collider other)
        {
            if(!isStop && 1 << other.gameObject.layer == wallLayer)
            {
                lastPosition = transform.position;
                isStop = true;
                playerController.HandMotion.WallCheck(isRight, isStop);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (1 << other.gameObject.layer == wallLayer)
            {
                isStop = false;
                playerController.HandMotion.WallCheck(isRight, isStop);
            }
        }
    }

}
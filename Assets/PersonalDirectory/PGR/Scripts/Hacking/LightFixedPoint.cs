using UnityEngine;

namespace PGR
{
    public class LightFixedPoint : MonoBehaviour
    {
        [SerializeField] LineRenderer lineRenderer;
        [SerializeField] Transform movablePoint;

        void LateUpdate()
        {
            if (movablePoint == null)
                return;

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, movablePoint.position);
        }

        public void SetLine(Transform target)
        {
            movablePoint = target;
        }
    }

}
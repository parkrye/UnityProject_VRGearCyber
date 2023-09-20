using UnityEngine;

namespace PGR
{
    public class FixedPoint : HackingPointBase
    {
        [SerializeField] HackingPuzzle hackingPuzzle;
        [SerializeField] Transform mpTransform, lfpTransform1, lfpTransform2;
        [SerializeField] Renderer rn;
        [SerializeField] Material offMat, onMat;
        [SerializeField] float maximum1, maximum2, compare1, compare2, height;

        void LateUpdate()
        {
            if (hackingPuzzle == null)
                return;

            if (IsInArea())
            {
                rn.material = onMat;
                hackingPuzzle.TurnOn(this);
            }
            else
            {
                rn.material = offMat;
                hackingPuzzle.TurnOff(this);
            }
        }

        public void SetLight(HackingPuzzle _hackingPuzzle, Transform _mpTransform, Transform _lfpTransform1, Transform _lfpTransform2)
        {
            hackingPuzzle = _hackingPuzzle;
            mpTransform = _mpTransform;
            lfpTransform1 = _lfpTransform1;
            lfpTransform2 = _lfpTransform2;
        }

        bool IsInArea()
        {
            // mp, lfp1를 밑변으로 하고 높이가 .05인 삼각형의 넓이 * 2
            maximum1 = Vector3.Distance(mpTransform.position, lfpTransform1.position) * 0.05f;
            // mp-lfp1과 this 사이의 거리
            height = GetDictance(mpTransform.position, lfpTransform1.position, transform.position);
            // this, lfp1를 밑변으로 하고 높이가 1인 삼각형의 넓이 * 2
            compare1 = (Vector3.Distance(mpTransform.position, transform.position) + Vector3.Distance(lfpTransform1.position, transform.position)) * height;
            // 후자가 전자 이하라면 점은 범위 내에 있다
            if (compare1 <= maximum1)
                return true;

            // mp, lfp2를 밑변으로 하고 높이가 .05인 삼각형의 넓이 * 2
            maximum2 = Vector3.Distance(mpTransform.position, lfpTransform2.position) * 0.05f;
            // mp-lfp2과 this 사이의 거리
            height = GetDictance(mpTransform.position, lfpTransform2.position, transform.position);
            // this, lfp2를 밑변으로 하고 높이가 1인 삼각형의 넓이 * 2
            compare2 = (Vector3.Distance(mpTransform.position, transform.position) + Vector3.Distance(lfpTransform2.position, transform.position)) * height;
            // 후자가 전자 이하라면 점은 범위 내에 있다
            if (compare2 <= maximum2)
                return true;

            // 둘 다 아니라면 점은 범위 밖에 있다
            return false;
        }

        float GetDictance(Vector3 pointA, Vector3 pointB, Vector3 point)
        {
            Vector3 AtoB = pointB - pointA;
            return (Vector3.Cross(point - pointA, AtoB).magnitude / AtoB.magnitude);
        }
    }

}
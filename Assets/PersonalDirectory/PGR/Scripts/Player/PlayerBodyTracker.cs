using UnityEngine;

namespace PGR
{
    public class PlayerBodyTracker : MonoBehaviour
    {
        [SerializeField] Transform spineCollider, leftArmCollider, rightArmCollider;
        [SerializeField] Transform headTransform, feetTransform, leftHandTransform, rightHandTransform;

        void LateUpdate()
        {
            spineCollider.position = (headTransform.position * 4f + feetTransform.position) * 0.2f;
            leftArmCollider.position = (spineCollider.transform.position + leftHandTransform.position) * 0.5f;
            rightArmCollider.position = (spineCollider.transform.position + rightHandTransform.position) * 0.5f;
        }
    }

}
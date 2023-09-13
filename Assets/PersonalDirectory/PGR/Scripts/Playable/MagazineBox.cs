using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class MagazineBox : MonoBehaviour
    {
        [SerializeField] GameObject magazine;
        [SerializeField] bool isHovering;
        [SerializeField] Transform instantTransform;

        public void HoverEnterEvent(HoverEnterEventArgs args)
        {
            if (!isHovering)
            {
                if (args.interactorObject.transform.GetComponent<CustomDirectInteractor>())
                {
                    isHovering = true;
                    GameManager.Resource.Instantiate(magazine, instantTransform.position, Quaternion.identity, true);
                }
            }
        }

        public void HoverExitEvent(HoverExitEventArgs args)
        {
            if (isHovering)
            {
                if (args.interactorObject.transform.GetComponent<CustomDirectInteractor>())
                {
                    isHovering = false;
                }
            }
        }
    }
}

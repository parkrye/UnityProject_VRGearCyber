using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PGR
{
    public class OnableLamp : MonoBehaviour
    {
        [SerializeField] bool isOn;
        [SerializeField] MeshRenderer mr;
        [SerializeField] Material onMat, offMat;

        void Awake()
        {
            mr.material = offMat;
        }

        public void OnHoverEntered(HoverEnterEventArgs hoverEnterEventArgs)
        {
            isOn = !isOn;
            if (isOn)
            {
                mr.material = onMat;
            }
            else
            {
                mr.material = offMat;
            }
        }
    }

}
using UnityEngine;
using UnityEngine.Rendering;

namespace PGR
{
    public class PlayerSight : MonoBehaviour
    {
        [SerializeField] Volume volume;

        public void LightVolume(bool isLightOn)
        {
            volume.enabled = isLightOn;
        }
    }
}
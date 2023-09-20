using UnityEngine;

namespace PGR
{
    public class AreaLight : MonoBehaviour
    {
        [SerializeField] Light[] lights;
        [SerializeField] int isPlayerInArea;

        void Awake()
        {
            lights = GetComponentsInChildren<Light>();

            foreach (Light light in lights)
            {
                light.enabled = false;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                isPlayerInArea++;
                if(isPlayerInArea > 0)
                {
                    foreach(Light light in lights)
                    {
                        light.enabled = true;
                    }
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                isPlayerInArea--;
                if (isPlayerInArea == 0)
                {
                    foreach (Light light in lights)
                    {
                        light.enabled = false;
                    }
                }
            }
        }
    }

}
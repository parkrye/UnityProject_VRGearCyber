using UnityEngine;

namespace PGR
{
    public class LightLever : MonoBehaviour
    {
        void Start()
        {
            PlayerPrefs.SetInt("Light", 0);
        }

        public void LeverDown()
        {
            PlayerPrefs.SetInt("Light", 1);
        }

        public void LeverUp()
        {
            PlayerPrefs.SetInt("Light", 0);
        }
    }

}
using UnityEngine;

namespace PGR
{
    public class MiniGolfHole : MonoBehaviour
    {
        [SerializeField] MiniGolf miniGolf;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject != miniGolf.Ball.gameObject)
                return;

            miniGolf.HoleIn();
        }
    }

}
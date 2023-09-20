using UnityEngine;

namespace PGR
{
    public class MiniGolf : MonoBehaviour
    {
        [SerializeField] Transform ball;
        [SerializeField] Vector3 InitialPosition;
        public Transform Ball {  get { return ball; } }

        void Start()
        {
            InitialPosition = ball.position;
        }

        public void HoleIn()
        {
            ball.transform.position = InitialPosition;
        }
    }
}
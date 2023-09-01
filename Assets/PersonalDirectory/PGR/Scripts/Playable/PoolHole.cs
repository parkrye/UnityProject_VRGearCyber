using UnityEngine;

namespace PGR
{
    public class PoolHole : MonoBehaviour
    {
        [SerializeField] PoolGame poolGame;
        [SerializeField] string poolBallName;

        void OnTriggerEnter(Collider other)
        {
            if (!other.name.Equals(poolBallName))
                return;

            other.gameObject.SetActive(false);
            poolGame.BallRemoved();
        }
    }
}

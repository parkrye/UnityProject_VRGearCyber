using System.Collections.Generic;
using UnityEngine;

namespace PGR
{
    public class PoolGame : MonoBehaviour
    {
        [SerializeField] int totalBallCount, removedBallCount;
        [SerializeField] SphereCollider[] balls;
        [SerializeField] Dictionary<SphereCollider, Vector3> ballDict;

        private void Awake()
        {
            balls = GetComponentsInChildren<SphereCollider>();
            ballDict = new Dictionary<SphereCollider, Vector3>();
            foreach(SphereCollider ball in balls)
            {
                ballDict.Add(ball, ball.transform.position);
            }
        }

        public void BallRemoved()
        {
            removedBallCount++;
            if(removedBallCount == totalBallCount)
            {
                removedBallCount = 0;
                foreach(SphereCollider ball in balls)
                {
                    ball.transform.position = ballDict[ball];
                    ball.gameObject.SetActive(true);
                }
            }
        }
    }

}
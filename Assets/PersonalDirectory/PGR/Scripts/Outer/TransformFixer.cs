using System.Collections;
using UnityEngine;

namespace PGR
{
    public class TransformFixer : MonoBehaviour
    {
        [SerializeField] Transform playerTransform;

        void Start()
        {
            StartCoroutine(FollowRoutine());
        }

        IEnumerator FollowRoutine()
        {
            yield return null;

            playerTransform = GameManager.Data.Player.IrisSystem.transform;
            while(true)
            {
                transform.position = playerTransform.position;
                yield return new WaitForFixedUpdate();
            }
        }
    }

}
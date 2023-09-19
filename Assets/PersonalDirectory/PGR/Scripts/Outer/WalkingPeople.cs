using System.Collections;
using UnityEngine;

namespace PGR
{
    public class WalkingPeople : MonoBehaviour
    {
        [SerializeField] Transform[] transforms;
        [SerializeField] GameObject[] peoples;
        [SerializeField] int nowPeople, maxPeople;

        void Start()
        {
            StartCoroutine(SpawnRoutine());
        }

        IEnumerator SpawnRoutine()
        {
            while (true)
            {
                if(nowPeople < maxPeople)
                {
                    GameObject person = GameManager.Resource.Instantiate(peoples[Random.Range(0, peoples.Length)], transforms[nowPeople % 2].position, Quaternion.identity, transform);
                    person?.GetComponent<NormalCityPeople>()?.SetDest(this, transforms[nowPeople % 2], transforms[(nowPeople + 1) % 2]);
                    nowPeople++;
                }
                yield return new WaitForSeconds(5f);
            }
        }

        public void PersonGoHome()
        {
            nowPeople--;
        }
    }
}
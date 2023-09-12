using System.Collections;
using UnityEngine;

namespace PGR
{
    public class RushHour : MonoBehaviour
    {
        [SerializeField] CityCar[] cars;
        [SerializeField] Transform[] points;
        [SerializeField] int nowCar, maxCar;

        void Start()
        {
            StartCoroutine(SpawnCarRoutine());
        }

        IEnumerator SpawnCarRoutine()
        {
            while (true)
            {
                yield return new WaitUntil(() => nowCar < maxCar);

                nowCar++;

                int startPosition = Random.Range(0, points.Length);
                int endPosition = Random.Range(0, points.Length);
                while (startPosition == endPosition)
                    endPosition = Random.Range(0, points.Length);

                CityCar car = GameManager.Resource.Instantiate(cars[Random.Range(0, cars.Length)]);
                car.transform.SetParent(transform);
                car.transform.position = points[startPosition].position;

                car.SettingAgent(points[endPosition].position);
                car.ArriveEvent.AddListener(Arrive);

                yield return new WaitForSeconds(4f);
            }
        }
        
        public void Arrive()
        {
            nowCar--;
        }
    }
}

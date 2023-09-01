using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGR
{
    public class HackingPuzzle : MonoBehaviour
    {
        [SerializeField] GameObject movablePoint, lightFixedPoint, fixedPoint;

        [SerializeField] int pairCount, fixedPointPerPairCount;
        [SerializeField] Dictionary<GameObject, List<GameObject>> lightConnectionDict;
        [SerializeField] Dictionary<GameObject, (LineRenderer, LineRenderer)> lineRendererDict;

        public void InitialPuzzle()
        {
            lightConnectionDict = new Dictionary<GameObject, List<GameObject>>();

            for (int i = 0; i < pairCount; i++)
            {
                List<GameObject> connectedList = new();
                // Instantiate movablePoint, lightFixedPoint * 2
                // connectedList.Add(movable, lightFixedPoint * 2)
                for(int j = 0; j < fixedPointPerPairCount; j++)
                {
                    // Instantiate fixedPoint
                    // connectedList.Add(fixedPoint)
                }
                // lightConnectionDict.Add(movablePoint, connectedList);
            }
        }
    }

}
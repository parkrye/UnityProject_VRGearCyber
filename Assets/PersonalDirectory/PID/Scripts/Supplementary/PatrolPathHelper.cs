using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PID
{
    public static class PatrolPathHelper
    {
        const int INF = 99999999;
        public static void ShortestPath(in Vector3[] graph, in Vector3 start, ref PriorityQueue<DestinationPoint> patrolQueue)
        {
            int size = graph.Length;
            int counter = 0; 
            bool[] visited = new bool[size];
            Vector3 prevPoint = graph[0]; 
            float deltaDist; 
            while (size >= 1)
            {
                deltaDist = Vector3.SqrMagnitude(graph[counter] - start);
                DestinationPoint nextPatrolPoint = new DestinationPoint(graph[counter], deltaDist);
                patrolQueue.Enqueue(nextPatrolPoint); 
                prevPoint = graph[counter++]; 
                --size;
            }
        }

        //public static PriorityQueue<DestinationPoint> GeneratePatrolQueue(in Vector3 start, in Vector3[] patrolPoints)
        //{

        //}
    }
}
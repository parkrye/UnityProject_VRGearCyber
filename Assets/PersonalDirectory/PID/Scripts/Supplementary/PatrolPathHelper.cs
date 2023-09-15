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

        public static float[,] GenerateSqrMgDistGraph(Vector3[] patrolPoints, Vector3 startPoint)
        {
            int size = patrolPoints.Length + 1;
            float[,] travelGraph = new float[size,size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == j)
                    {
                        travelGraph[i, j] = 0; // Distance to itself is 0
                    }
                    else if (i == 0)
                    {
                        // Distance from the starting point to patrol points
                        travelGraph[i, j] = Vector3.SqrMagnitude(startPoint - patrolPoints[j - 1]);
                    }
                    else if (j == 0)
                    {
                        // Distance from patrol points to the starting point
                        travelGraph[i, j] = Vector3.SqrMagnitude(patrolPoints[i - 1] - startPoint);
                    }
                    else
                    {
                        // Distance between two patrol points
                        travelGraph[i, j] = Vector3.SqrMagnitude(patrolPoints[i - 1] - patrolPoints[j - 1]);
                    }
                }
            }
            return travelGraph;
        }

    }
}
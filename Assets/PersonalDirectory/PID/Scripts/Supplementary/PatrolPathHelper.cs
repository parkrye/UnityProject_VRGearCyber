using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PID
{
    public static class PatrolPathHelper
    {
        const int INF = 99999999;
        public static void ShortestPath(in Vector3[] graph, in Vector3 start, out int[] distance, out Vector3[] path)
        {
            int size = graph.Length;
            bool[] visited = new bool[size];
            distance = new int[size];
            path = new Vector3[size];

            for (int i = 0; i < size; i++)
            {
                distance[i] = INF;
                path[i] = Vector3.zero; 
                visited[i] = false;
            }
            distance[0] = 0;

            for (int i = 0; i < size; i++)
            {
                // 1. 방문하지 않은 정점 중 가장 가까운 정점부터 탐색
                int next = -1;
                int minCost = INF;
                for (int j = 0; j < size; j++)
                {
                    if (!visited[j] &&
                        distance[j] < minCost)
                    {
                        next = j;
                        minCost = distance[j];
                    }
                }
                if (next < 0)
                    break;

                // 2. 직접연결된 거리보다 거쳐서 더 짧아진다면 갱신.
                //for (int j = 0; j < size; j++)
                //{
                //    if (i == j)
                //        return; 
                //    // distance[j] : 목적지까지 직접 연결된 거리
                //    // distance[next] : 탐색중인 정점까지 거리
                //    // graph[next, j] : 탐색중인 정점부터 목적지의 거리
                //    if (distance[j] > distance[next] + graph[next, j])
                //    {
                //        distance[j] = distance[next] + graph[next, j];
                //        path[j] = next;
                //    }
                //}
                visited[next] = true;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static PID.RobotHelper; 

namespace PID
{
    public class SoundTraceHelper
    {
        const int wallPenalty = 2; 

        
        public void ComputeTracePath(NavMeshAgent agent, Vector3 soundPoint)
        {
            NavMeshPath samplePath = new NavMeshPath();
            if (NavMesh.CalculatePath(agent.transform.position, soundPoint, agent.areaMask, samplePath))
            {
                //Check for distance validity 
                if (ValidSound(samplePath, soundPoint, agent.transform.position))
                {
                    GameManager.traceSound.FinishedProcessingPath(soundPoint, true);
                }
                else
                    GameManager.traceSound.FinishedProcessingPath(soundPoint, false);
            }
            else
            {
                GameManager.traceSound.FinishedProcessingPath(soundPoint, false); 
            }
        }
        /// <summary>
        /// Compares Straight distance vs Actual path to travel. 
        /// if actual path >= straight path * 2, then returns false, else true; 
        /// </summary>
        /// <returns></returns>
        public bool ValidSound(NavMeshPath path, Vector3 destinationPoint, Vector3 agentPoint)
        {
            float straightPath = Vector3.SqrMagnitude(destinationPoint - agentPoint);
            float actualPath = CalculateDistance(path, agentPoint);

            return straightPath* wallPenalty >= actualPath;
        }
        public float CalculateDistance(NavMeshPath path, Vector3 agentPoint)
        {
            float distance = 0f;
            Vector3 prevPoint = agentPoint; 
            for (int i = 0; i < path.corners.Length; i++ )
            {
                distance += Vector3.SqrMagnitude(path.corners[i] - prevPoint); 
                prevPoint = path.corners[i];
            }
            return distance;
        }
    }
}
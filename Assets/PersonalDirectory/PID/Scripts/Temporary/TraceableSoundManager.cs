using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PID
{
    public class TraceableSoundManager: MonoBehaviour
    {
        Queue<PathRequest> pathRequests = new Queue<PathRequest>();
        PathRequest currentPath;
        SoundTraceHelper traceHelper = new SoundTraceHelper();
        private bool isProcessing;
        public bool IsProcessing
        {
            get { return isProcessing; }
        }

        public bool WallIntersect(Vector3 soundPoint, Vector3 listenerPoint)
        {
            RaycastHit hitInfo;
            float sqrDist;
            Vector3 dir = soundPoint - listenerPoint;
            sqrDist = dir.sqrMagnitude;
            if (Physics.Raycast(listenerPoint, dir.normalized, out hitInfo, LayerMask.GetMask("Wall")))
            {
                if (hitInfo.distance * 2 > sqrDist)
                    return false;
                return true;
            }
            return false;
        }

        public void RequestPath(NavMeshAgent agent, Vector3 SoundPoint, Action<Vector3, bool> callback)
        {
            PathRequest newRequest = new PathRequest(agent, SoundPoint, callback);
            pathRequests.Enqueue(newRequest);
            TryProcessNext();
        }

        void TryProcessNext()
        {
            if (!isProcessing && pathRequests.Count > 0)
            {
                currentPath = pathRequests.Dequeue();
                isProcessing = true;
                traceHelper.ComputeTracePath(currentPath.agent, currentPath.soundPoint);
            }
        }

        public void FinishedProcessingPath(Vector3 destinationPoint, bool success)
        {
            if (success)
                currentPath.callback(destinationPoint, success);
            isProcessing = false;
            TryProcessNext();
        }

        struct PathRequest
        {
            public NavMeshAgent agent; 
            public Vector3 soundPoint;
            public Action<Vector3, bool> callback;

            public PathRequest(NavMeshAgent agent, Vector3 end, Action<Vector3, bool> callback)
            {
                this.agent = agent; 
                this.soundPoint = end;
                this.callback =  callback;
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PID
{
    public static class RobotHelper
    {
        const float nearIntersectThreshold = .985f;
        public static Vector3 ShotCentrePoint(Vector3 muzzlePoint, Vector3 targetPoint, float distance)
        {
            // a. Vector3 normalized one 
            // b. Vector3 which determines the length of the normalized Vector3 
            // c. Dot (a * b) = length from O -> a in length of B. 
            // d. Dot (a * c) = Vector3 point of c. 
            Vector3 towardTargetDir = (targetPoint - muzzlePoint).normalized;
            
            Vector3 shotCentrePointWithDistance = towardTargetDir * distance;
            return shotCentrePointWithDistance;
        }

        public static Vector3 FinalShotPoint(Vector3 targetPoint, float distance, float randomPercentage)
        {
            targetPoint += UnityEngine.Random.insideUnitSphere * randomPercentage;
            return targetPoint;
        }

        public static Vector3 GroupPositionAllocator (Vector3 centrePoint, int size, int index)
        {
            float angularStart = (360.0f / size) * index; 
            float x = 10.0f * Mathf.Sin(angularStart * Mathf.Deg2Rad);
            float z = 10.0f * Mathf.Cos(angularStart * Mathf.Deg2Rad);
            Vector3 position = new Vector3(x, 0.0f, z);
            position.x += centrePoint.x; 
            position.z += centrePoint.z;
            return position;
        }
        public static bool DirectionIntersect(Vector3 dir_1, Vector3 dir_2)
        {
            if (Vector3.Dot(dir_1, dir_2) >= nearIntersectThreshold)
                return true;
            return false;
        }

        //public static bool TraversableSound()
        //{
        //    Vector3 prev = startingPoint.transform.position;
        //    float threshHold = 2 * Vector3.SqrMagnitude(destinationPoint - startingPoint.transform.position);
        //    float accumDist = 0f;
        //    foreach (Vector3 point in soundPath.corners)
        //    {
        //        //Calculate distance between each points. 
        //        float distance = Vector3.SqrMagnitude(point - prev);
        //        accumDist += distance;
        //        prev = point;
        //    }
        //    return (accumDist <= threshHold);
        //}
    }

    /// <summary>
    /// Returns -1 if current Point is shorter than the 'other'.point
    /// Returns 1 if current Point is further thant he 'other'.point
    /// Returns 0 if have equal distance; 
    /// </summary>
    public struct DestinationPoint : IComparable<DestinationPoint>
    {
        public Vector3 destinationVectorPoint;
        public float distanceToPoint; 

        public DestinationPoint (Vector3 dVP, float dTP)
        {
            this.destinationVectorPoint = dVP;
            this.distanceToPoint = dTP;
        }

        public int CompareTo(DestinationPoint other)
        {
            if (this.distanceToPoint < other.distanceToPoint) 
                return -1;
            else if (this.distanceToPoint > other.distanceToPoint)
                return 1;
            else 
                return 0;
        }
    }

    public struct SoundValidityCheckSlip
    {
        public NavMeshAgent startingPoint;
        public Vector3 destinationPoint;
        public NavMeshPath soundPath;
        public bool isValid; 

        public SoundValidityCheckSlip(NavMeshAgent startPoint, Vector3 destination, NavMeshPath soundPath)
        {
            this.startingPoint = startPoint;
            this.destinationPoint = destination;
            this.soundPath = soundPath;
            this.isValid = false;

            if (NavMesh.FindClosestEdge(destination, out NavMeshHit hitInfo, startPoint.areaMask))
            {
                this.isValid = true; 
            }
            else
            {
                Debug.Log("Hit Point not under valid navmesh region");
                isValid = false; 
            }
        }
    }
}


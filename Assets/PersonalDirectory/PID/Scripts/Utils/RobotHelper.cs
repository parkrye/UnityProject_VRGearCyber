using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PID
{
    public static class RobotHelper
    {
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

        public static Vector3 FinalShotDir(Vector3 muzzlePoint, Vector3 targetPoint, float distance, float randomPercentage)
        {
            Vector3 initialPoint = ShotCentrePoint(muzzlePoint, targetPoint, distance);
            Vector3 newPointer = UnityEngine.Random.insideUnitCircle * randomPercentage;
            newPointer.z = initialPoint.z;
            newPointer.y += initialPoint.y;
            newPointer.x += initialPoint.x;
            return (newPointer - muzzlePoint).normalized;
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
}


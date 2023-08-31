using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PID
{
    public static class RobotCombatHelper
    {
        public static Vector3 ShotCentrePoint(Vector3 muzzlePointForward, float distance)
        {
            // a. Vector3 normalized one 
            // b. Vector3 which determines the length of the normalized Vector3 
            // c. Dot (a * b) = length from O -> a in length of B. 
            // d. Dot (a * c) = Vector3 point of c. 
            Vector3 shotCentrePointWithDistance = muzzlePointForward * distance;
            return shotCentrePointWithDistance;
        }

        public static Vector3 FinalShotWithRandom(Vector3 muzzlePointForward, float distance, float randomPercentage)
        {
            Vector3 initialPoint = ShotCentrePoint(muzzlePointForward, distance);
            Vector3 newPointer = UnityEngine.Random.insideUnitCircle * randomPercentage;
            newPointer.z = initialPoint.z;
            newPointer.y += initialPoint.y;
            newPointer.x += initialPoint.x;
            return newPointer;
        }
    }

    /// <summary>
    /// Returns -1 if current Point is shorter than the 'other'.point
    /// Returns 1 if current Point is further thant he 'other'.point
    /// Returns 0 if have equal distance; 
    /// </summary>
    public struct DestinationPoint : IComparable<DestinationPoint>
    {
        Vector3 destinationVectorPoint;
        float distanceToPoint; 

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


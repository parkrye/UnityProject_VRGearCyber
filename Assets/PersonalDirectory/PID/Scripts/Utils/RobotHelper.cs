using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PID
{
    public static class RobotHelper
    {
        public enum RobotType
        {
            Guard, 
            Tackler, 
            Scouter
        }
        public enum State
        {
            Idle,
            Infiltrated,
            Patrol,
            LookAround,
            Alarm,
            Alert,
            Assault,
            Trace,
            SoundReact,
            Hide,
            Retrieve,
            Neutralized,
            Size
            //Possibly extending beyond for Gathering Abilities. 
        }
        const float nearIntersectThreshold = .985f;
        const float validSoundRegion = 2f;
        public const float untoDestThreshold = 1.41f; 

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
            float x = 1 * Mathf.Sin(angularStart * Mathf.Deg2Rad);
            float z = 1 * Mathf.Cos(angularStart * Mathf.Deg2Rad);
            Vector3 position = new Vector3(x, 0.0f, z);
            position.x += centrePoint.x; 
            position.z += centrePoint.z;
            return CheckValidPoint(position);
        }
        const float minimumDist = .5f;
        const float middleDist = 1;
        const float maximumDist = 1.5f; 
        public static Vector3 CheckValidPoint(Vector3 point)
        {
            if (NavMesh.SamplePosition(point, out NavMeshHit hit, minimumDist, NavMesh.AllAreas))
            {
                return hit.position; 
            }
            else if (NavMesh.SamplePosition(point, out NavMeshHit hit_2, middleDist, NavMesh.AllAreas)) 
            {
                return hit_2.position;  
            }
            else if (NavMesh.SamplePosition(point, out NavMeshHit hit_3, maximumDist, NavMesh.AllAreas))
            {
                return hit_3.position; 
            }
            else 
                return Vector3.zero;
        }
        public static bool DirectionIntersect(Vector3 dir_1, Vector3 dir_2)
        {
            if (Vector3.Dot(dir_1, dir_2) >= nearIntersectThreshold)
                return true;
            return false;
        }

        public static ValidSoundCheckSlip ValidSoundPoint(Vector3 soundPoint)
        {
            ValidSoundCheckSlip checkSlip;
            if (NavMesh.SamplePosition(soundPoint, out NavMeshHit hit, validSoundRegion, NavMesh.AllAreas))
            {
                return checkSlip = new ValidSoundCheckSlip(true, hit.position);
            }
            return checkSlip = new ValidSoundCheckSlip(false, Vector3.zero);
        }

        public static void LookDirToPlayer(Vector3 playerLoc, Transform robot, out Vector3 lookDir)
        {
            lookDir = (playerLoc - robot.position).normalized;
            lookDir.y = 0; 
        }
        
        public static Rigidbody NearestHitPart(Rigidbody[] parts, Vector3 hitPoint)
        {
            if (hitPoint == Vector3.zero)
                return null; 
            float shortest = 9999999999999999f;  
            float deltaDist;
            Rigidbody closest = null;
            for (int i = 0; i< parts.Length; i++)
            {
                deltaDist = Vector3.SqrMagnitude(parts[i].transform.position - hitPoint);
                if (deltaDist < shortest)
                {
                    closest = parts[i];
                    shortest = deltaDist;
                }
            }
            return closest; 
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
            else //If they are the same, 
            {
                return 0; 
            }
                
        }
    }

    public struct ValidSoundCheckSlip
    {
        public bool isValid;
        public Vector3 soundPointOnPath;

        public ValidSoundCheckSlip(bool isValid, Vector3 soundPointOnPath)
        {
            this.isValid = isValid;
            this.soundPointOnPath = soundPointOnPath;
        }
    }
}
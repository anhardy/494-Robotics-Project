using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Flock/Behavior/Follow")]
public class Follow : FlockingBehavior
{
    public override Vector3 CalculateDirection(Feesh feesh, List<Transform> nearby, Flock flock)
    {
        if (flock.feeshLeaderPrefab != null)
        {
            float distanceToFollowTo = flock.followToRadius;
            Feesh feeshLeader = flock.GetLeader;
            float distanceToLeader = Vector3.Distance(feeshLeader.transform.position, feesh.transform.position); //Get distance from our fish to leader
            Vector3 followDirection = Vector3.zero;
            if(distanceToLeader > distanceToFollowTo) {
                followDirection = feeshLeader.transform.position - feesh.transform.position;
            }

            return followDirection;
        }
        return Vector3.zero;
    }
}



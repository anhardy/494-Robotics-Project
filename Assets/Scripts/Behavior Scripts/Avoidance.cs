using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class Avoidance : FlockingBehavior
{
    Vector3 currentVelocity;
    public float feeshSmoothTime = 0.5f; //How long to get from current state to caluclated state
    public override Vector3 CalculateDirection(Feesh feesh, List<Transform> nearby, Flock flock)
    {

        if (nearby.Count == 0)
        { //If no fish nearby
            return Vector3.zero; //No adjustment necessary
        }
        Vector3 avoidanceDirection = Vector3.zero; //Direction to move to stay avoid neighbors, initialized to zero
        int collisionCount = 0; //How many neighbors are in collision radius
        foreach (Transform nearbyFeesh in nearby)
        {
            if (Vector3.SqrMagnitude(nearbyFeesh.position - feesh.transform.position) < flock.getSquareAvoidanceRadius)
            { //If the square distance between current feesh and nearby feesh is within avoidance radius
                collisionCount++;
                avoidanceDirection += feesh.transform.position - nearbyFeesh.position; //Current position minus position of offending feesh. This also calculates offset from global position       
            }
        }

        if (flock.GetLeader != null)

            if (collisionCount > 0)
            { //No divide by zero please
                avoidanceDirection = avoidanceDirection / collisionCount; //Average out positions. 
            }

        avoidanceDirection = Vector3.SmoothDamp(feesh.transform.forward, avoidanceDirection, ref currentVelocity, feeshSmoothTime);

        return avoidanceDirection;
    }
}

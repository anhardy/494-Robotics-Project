using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Flock/Behavior/SteeredCohesion")]
public class SteeredCohesion : FlockingBehavior
{
    Vector3 currentVelocity;
    public float feeshSmoothTime = 0.5f; //How long to get from current state to caluclated state
    public override Vector3 CalculateDirection(Feesh feesh, List<Transform> nearby, Flock flock)
    {
        if(nearby.Count == 0) { //If no fish nearby
            return Vector3.zero; //No adjustment necessary
        }
        Vector3 cohesionDirection = Vector3.zero; //Direction to move to stay in cohesion, initialized to zero
        foreach(Transform nearbyFeesh in nearby) {
            cohesionDirection += nearbyFeesh.position; //Add position of each nearby feesh           
        }
        cohesionDirection = cohesionDirection / nearby.Count; //Average out positions. However, this is a global position
        cohesionDirection -= feesh.transform.position; //Offset from current feesh position

        //Take current direction vector, direction vector of cohesion movement, our current velocity, and apply our smoothing
        cohesionDirection = Vector3.SmoothDamp(feesh.transform.forward, cohesionDirection, ref currentVelocity, feeshSmoothTime); 

        return cohesionDirection;
    }
}

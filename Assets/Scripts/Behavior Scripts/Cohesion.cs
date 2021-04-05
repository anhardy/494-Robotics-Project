using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Cohesion")]
public class Cohesion : FlockingBehavior //Inherits Flocking Behavior
{

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

        return cohesionDirection;
    }

    
}

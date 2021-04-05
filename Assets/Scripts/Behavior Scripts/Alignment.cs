using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Alignment")]
public class Alignment : FlockingBehavior
{
     public override Vector3 CalculateDirection(Feesh feesh, List<Transform> nearby, Flock flock)
    {
        if(nearby.Count == 0) { //If no fish nearby
            return feesh.transform.forward; //Maintain current direction
        }
        Vector3 alignmentDirection = Vector3.zero; //Direction to move to stay in alignment, initialized to zero
        foreach(Transform nearbyFeesh in nearby) {
            alignmentDirection += nearbyFeesh.forward; //Add direction of each nearby fish  
        }
        alignmentDirection = alignmentDirection / nearby.Count; //Average out direction.
        //Direction is independent of position. No need to offset.

        return alignmentDirection;
    }
}

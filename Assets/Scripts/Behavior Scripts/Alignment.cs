using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Alignment")]
public class Alignment : FlockingBehavior
{
    public override Vector3 CalculateDirection(Feesh feesh, List<Transform> nearby, Flock flock)
    {
        if (feesh.NearbyCount == 0)
        { //If no fish nearby
            return feesh.transform.forward; //Maintain current direction
        }
        Vector3 alignmentDirection = Vector3.zero; //Direction to move to stay in alignment, initialized to zero
        foreach (Transform nearbyFeesh in nearby)
        {
            if (!nearbyFeesh.gameObject.layer.Equals(LayerMask.NameToLayer("Obstacle"))) //If not an obstacle
            {
                alignmentDirection += nearbyFeesh.forward; //Add direction of each nearby fish  
            }
        }
        alignmentDirection = alignmentDirection / feesh.NearbyCount; //Average out direction.
        //Direction is independent of position. No need to offset.

        return alignmentDirection;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Seek : SurvivalBehavior
{
    Vector3 currentVelocity;
    public float feeshSmoothTime = 0.01f; //How long to get from current state to caluclated state
    public float SEEK_VELOCITY = 5f;

    // SEEKS CLOSEST FEESH
    public override Vector3 CalculateDirection(Shork shork, List<Transform> nearby)
    {
        if (shork.NearbyCount == 0)
        { //If no fish nearby
            return shork.transform.forward; //Maintain current direction
        }
        Transform closestFeesh = null;
        float min = float.MaxValue; //Closest shork

        // FIND NEAREST FEESH
        foreach (Transform nearbyFeesh in nearby)
        {
            if (!nearbyFeesh.gameObject.layer.Equals(LayerMask.NameToLayer("Obstacle"))) //If not an obstacle
            {
                Vector3 distanceVect = shork.transform.position - nearbyFeesh.position;
                if (distanceVect.magnitude < min) { // found a feesh that's closer
                    closestFeesh = nearbyFeesh;
                    min = distanceVect.magnitude;
                }
            }
        }
        
        // SEEK TARGET
        Vector3 feeshVector = closestFeesh.forward * SEEK_VELOCITY;
        Vector3 distanceVector = closestFeesh.position - shork.transform.position;
        Vector3 seek = feeshVector + distanceVector;

        return seek;
    }
}



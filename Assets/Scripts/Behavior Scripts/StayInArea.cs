using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Stay in area")]

public class StayInArea : FlockingBehavior
{
    public override Vector3 CalculateDirection(Feesh feesh, List<Transform> nearby, Flock flock) { 
        Vector3 center = flock.transform.position; //Center is where flock is physically placed
        float radius = flock.stayWithinRadius; //Radius defined in Flock class.
        Vector3 centerOffeset = center - feesh.transform.position; //Offset from chosen center and current feesh/flock agent
        float t = centerOffeset.magnitude / radius;

        if(t < 0.9f) { //If within magnitude of radius
            return Vector3.zero; //Nothing needs to be done
        }

        return centerOffeset * t * t;
    }
   
}

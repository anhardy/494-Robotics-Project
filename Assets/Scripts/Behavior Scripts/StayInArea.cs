using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Stay in area")]

public class StayInArea : FlockingBehavior
{
    public Vector3 center;
    public float radius = 15f;
    public override Vector3 CalculateDirection(Feesh feesh, List<Transform> nearby, Flock flock) { 
        Vector3 centerOffeset = center - feesh.transform.position;
        float t = centerOffeset.magnitude / radius;

        if(t < 0.9f) { //Within magnitude of radius
            return Vector3.zero;
        }

        return centerOffeset * t * t;
    }
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockingBehavior : ScriptableObject
{
    public abstract Vector3 CalculateDirection (Feesh feesh,  List<Transform> nearby, Flock flock); 
}

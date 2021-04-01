using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockingBehavior : ScriptableObject
{
    public abstract Vector3 CalcDirection (Feesh feesh,  List<Transform> context, Flock flock); //Context list is list of neighbors around feesh.
}

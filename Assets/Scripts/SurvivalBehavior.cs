using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SurvivalBehavior : ScriptableObject
{
    public abstract Vector3 CalculateDirection (Shork feesh,  List<Transform> nearby); 
}

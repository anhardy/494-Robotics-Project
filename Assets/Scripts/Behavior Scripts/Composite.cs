using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =  "Flock/Behavior/Composite")]
public class Composite : FlockingBehavior
{
    public FlockingBehavior[] behaviors;
    public float[] behaviorWeights;
    public override Vector3 CalculateDirection(Feesh feesh, List<Transform> nearby, Flock flock)
    {
        if(behaviorWeights.Length != behaviors.Length) { //There's a better way to do this than ensuring we manually enter the same lengths
            Debug.LogError("You need the same number of weights as behaviors, dummy.");
            return Vector3.zero;
        }

        Vector3 direction = Vector3.zero;

        for(int i = 0; i < behaviors.Length; i++) {
            Vector3 partialDirection = behaviors[i].CalculateDirection(feesh, nearby, flock) * behaviorWeights[i]; //Calculate respective behavior with respective weight

            if(partialDirection != Vector3.zero) { //If there is some movement being returned
                if(partialDirection.sqrMagnitude > behaviorWeights[i] * behaviorWeights[i]) { //If the square magnitude of our partial direction is > our squared behavior weight
                    partialDirection = partialDirection.normalized;
                    partialDirection *= behaviorWeights[i]; //Set its direction to be that of the behavior weight;
                }

                direction += partialDirection; //Add to overall (composite) direction
            }
        }

        return direction;
    }

}

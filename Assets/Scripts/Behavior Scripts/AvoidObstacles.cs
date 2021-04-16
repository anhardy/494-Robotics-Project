using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoid Obstacles")]
public class AvoidObstacles : FlockingBehavior
{
    Vector3 currentVelocity;
    public float feeshSmoothTime = 0.5f; //How long to get from current state to caluclated state
    public override Vector3 CalculateDirection(Feesh feesh, List<Transform> nearbyObstacles, Flock flock)
    {

        if (nearbyObstacles.Count == 0)
        { //If nothing nearby
            return Vector3.zero; //No adjustment necessary
        }
        Vector3 avoidanceDirection = Vector3.zero; //Direction to move to stay avoid obstacles
        float biggestThreatDistance = float.MaxValue;
        Vector3 closestColliderPoint;
        Collider closestCollider;
        foreach (Transform nearby in nearbyObstacles)
        {
            if (nearby.gameObject.layer.Equals(LayerMask.NameToLayer("Obstacle"))) //If nearby is an obstacle
            {

                float distanceToObject = Vector3.Distance(nearby.transform.position, feesh.transform.position);
                if (distanceToObject < biggestThreatDistance)
                {
                    if (true)//nearby.gameObject.CompareTag("Terrain"))
                    {
                        biggestThreatDistance = distanceToObject;
                        avoidanceDirection = feesh.transform.position - nearby.position; //Current position minus position of obstacle feesh. This also calculates offset from global position  
                    } else { //Experimenting with getting nearest point on collider. Doesn't quite work right
                        closestCollider = nearby.GetComponent<Collider>();
                        closestColliderPoint = closestCollider.ClosestPointOnBounds(feesh.transform.position);
                        avoidanceDirection = -closestColliderPoint;
                    }
                }

            }
        }
        avoidanceDirection = Vector3.SmoothDamp(feesh.transform.forward, avoidanceDirection, ref currentVelocity, feeshSmoothTime);


        return avoidanceDirection;
    }
}

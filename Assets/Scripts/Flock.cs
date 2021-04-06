using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
  public Feesh feeshPrefab;
  List<Feesh> feeshes = new List<Feesh>();
  public FlockingBehavior behavior;
  [Range(1,1000)]
  public int flockCount = 250;
  const float feeshDensity = 0.08f;
  [Range(1f, 100f)]
  public float velocityMultiplier = 10f;
  [Range(1f,100f)]
  public float maxVelocity = 5f;
  [Range(1f, 10f)]
  public float neighborRadius = 1.5f;
  [Range(0f,1f)]
  public float avoidanceRadiusMultiplier = 0.5f;
  public bool useTags = false;

  float squareMaxVelocity;
  float squareNeighborRadius;
  float squareAvoidanceRadius;
  public float getSquareAvoidanceRadius { 
      get {
          return squareAvoidanceRadius;
      }
  }

  void Start() {
      squareMaxVelocity = maxVelocity * maxVelocity;
      squareNeighborRadius = neighborRadius * neighborRadius;
      squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

      for(int i = 0; i < flockCount; i++) { //For the number of feesh we want
          Feesh newFeesh = Instantiate(
              feeshPrefab, //Our feesh prefab
              Random.insideUnitSphere * flockCount * feeshDensity,  //A random position in sphere (with size based on number of feesh) to spawn it at
              Random.rotation, //Facing a random direction
              transform
          );
          newFeesh.name = "feesh " + i;
          feeshes.Add(newFeesh);
      }
  }

  void Update() {
        foreach(Feesh feesh in feeshes) { //Oh god, a loop for for every single frame.
            List<Transform> nearby = GetNearbyObjects(feesh); //List of everything near current feesh
            Vector3 direction = behavior.CalculateDirection(feesh, nearby, this); //Calculate our direction
            direction *= velocityMultiplier;
            if(direction.sqrMagnitude > squareMaxVelocity) { //If greater than max velocity
                direction = direction.normalized * maxVelocity; //Normalize (set it to 1) and set to max speed
            }
            feesh.Move(direction); //Move in direction
        }
    }
    List<Transform> GetNearbyObjects(Feesh feesh) {
        List<Transform> nearby = new List<Transform>(); //List of nearby transforms.
        Collider[] nearbyColliders = Physics.OverlapSphere(feesh.transform.position, neighborRadius); //Array of nearby sphere colliders around current fish within radius
        foreach(Collider collider in nearbyColliders) { //Another loop being ran inside a loop for every frame. I do not like this.
            if (collider != feesh.getFeeshCollider) { //If collider is not feesh's own collider 
                if(useTags == false) { //If flock indescriminately groups up
                    nearby.Add(collider.transform); //Add transform of nearby collider to nearby list
                } else if(feesh.CompareTag(collider.tag)) { //Otherwise filter by tag
                    nearby.Add(collider.transform);
                }
            }
        }

        return nearby;
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
  public Feesh feeshPrefab;
  List<Feesh> feeshes = new List<Feesh>();
  public FlockingBehavior behavior;
  [Range(1,1000)]
  [Tooltip("How many agents are in the flock")]
  public int flockCount = 250;
  const float feeshDensity = 0.08f;
  [Range(1f, 100f)]
  [Tooltip("Increase the velocity of the flock agents")]
  public float velocityMultiplier = 10f;
  [Range(1f,100f)]
  [Tooltip("The maximum velocity of the flock agents")]
  public float maxVelocity = 5f;
  [Range(1f, 10f)]
  [Tooltip("The radius agents will accept neighbors in")]
  public float neighborRadius = 1.5f;
  [Range(0f,1f)]
  [Tooltip("The radius agents will consider other agents to be too close")]
  public float avoidanceRadiusMultiplier = 0.5f;
  [Tooltip("Fish agents will group up with other fish agents who have the same tag, regardless of what Flock instance they are apart of, but ignore any other fish. This applies to the tags of the fish agent type the flock spawns, not the tag of the flock itself.")]
  public bool useTags = false; //Use this if you want your feesh to only group with other feesh of the same tag (can be outside this Flock object)
  [Tooltip("Fish agents will exclusively group up with fish spawned by their Flock class object. This takes takes precedence over tags.")]
  public bool stayWithinThisFlock = false; //Use this if you want your feesh to only stay within the same instance of the Flock class (takes priority over useTags)
  [Tooltip("Fish agents will respect the above two discriminators. If disabled, these agents will continue to group up with agents who are ignoring them")]
  public bool respectDiscriminators = true; //Use this if you want agents of this flock to ignore flocks with useTags or stayWithinThisFlock set to true;

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
          newFeesh.InitializeFlock(this); //This instance of the Flock class is the Flock this feesh belongs to
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
                if(useTags == false && stayWithinThisFlock == false) { //If flock indescriminately groups up
                    if(respectDiscriminators == false) { //If ignores discriminators of other flocks
                        nearby.Add(collider.transform); //Add transform of nearby collider to nearby list and follow anyways
                    } else { //If we respect flocks with discriminators
                        Feesh feeshInstance = collider.GetComponent<Feesh>(); //Get Feesh object from its collider
                        if(feeshInstance != null && feeshInstance.getFlock.stayWithinThisFlock == false && feeshInstance.getFlock.useTags == false) { //If collider belongs to a feesh and this feesh does not use discriminators
                            nearby.Add(collider.transform);
                        }
                    }
                } else if(stayWithinThisFlock == true) { //Else if grouping up by class instance
                    Feesh feeshInstance = collider.GetComponent<Feesh>(); 
                    if(feeshInstance != null && feeshInstance.getFlock == feesh.getFlock) { //If collider belongs to a feesh and is part of same class
                        nearby.Add(collider.transform); 
                    }
                }
                else if(feesh.CompareTag(collider.tag)) { //Otherwise filter by tag
                    //An agent with the same tag may belong to a different flock which wishes to stay within that class. Need to check 
                    if(respectDiscriminators == false) {  //If we ignore discriminators
                        nearby.Add(collider.transform);
                    } else { 
                        Feesh feeshInstance = collider.GetComponent<Feesh>();
                        if(feeshInstance != null && feeshInstance.getFlock.stayWithinThisFlock == false && feeshInstance.getFlock.useTags == false) { //If collider belongs to a feesh and this feesh does not use discriminators
                            nearby.Add(collider.transform);
                        }
                    }
                }
            }
        }

        return nearby;
    }



}

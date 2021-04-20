using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShorkManager : MonoBehaviour
{
    [Tooltip("Your general feesh flock agent.")]
    public Shork shorkPrefab;
    List<Shork> shorks = new List<Shork>();
    public SurvivalBehavior behavior;
    [Range(1, 1000)]
    [Tooltip("How many agents are in the flock")]
    public int flockCount = 250;
    const float feeshDensity = 0.08f;
    [Range(1f, 100f)]
    [Tooltip("Increase the velocity of the flock agents")]
    public float velocityMultiplier = 10f;
    [Range(1f, 100f)]
    [Tooltip("The maximum velocity of the flock agents")]
    public float maxVelocity = 5f;
    [Range(1f, 10f)]
    [Tooltip("The radius agents will accept neighbors in")]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    [Tooltip("The radius agents will consider other agents to be too close")]
    public float avoidanceRadiusMultiplier = 0.5f;
    [Range(0f, 100f)]
    [Tooltip("The radius agents will avoid obstacles")]
    public float obstacleAvoidanceRadius = 5f;
    [Range(0f, 100f)]
    [Tooltip("The radius of the leader the fish will follow up to")]
    public float followToRadius = 5f;
    [Tooltip("Apply movement smoothing when following a leader.")]
    public bool smoothFollow = false;
    [Tooltip("Fish agents will group up with other fish agents who have the same tag, regardless of what Flock instance they are apart of, but ignore any other fish. This applies to the tags of the fish agent type the flock spawns, not the tag of the flock itself.")]
    public bool useTags = false; //Use this if you want your feesh to only group with other feesh of the same tag (can be outside this Flock object)
    [Tooltip("Fish agents will exclusively group up with fish spawned by their Flock class object. This takes takes precedence over tags.")]
    public bool stayWithinThisFlock = false; //Use this if you want your feesh to only stay within the same instance of the Flock class (takes priority over useTags)
    [Tooltip("Fish agents will respect the above two discriminators. If disabled, these agents will continue to group up with agents who are ignoring them")]
    public bool respectDiscriminators = true; //Use this if you want agents of this flock to ignore flocks with useTags or stayWithinThisFlock set to true;
    Feesh newLeader;


    float squareMaxVelocity;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    float squareObstacleAvoidanceRadius;

    public Feesh GetLeader
    {
        get
        {
            return newLeader;
        }
    }
    public float getSquareAvoidanceRadius
    {
        get
        {
            return squareAvoidanceRadius;
        }
    }

    public float getSquareObstacleAvoidanceRadius
    {
        get
        {
            return squareObstacleAvoidanceRadius;
        }
    }

    void Start()
    {
        squareMaxVelocity = maxVelocity * maxVelocity;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareObstacleAvoidanceRadius = obstacleAvoidanceRadius * obstacleAvoidanceRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < flockCount; i++)
        { //For the number of feesh we want
            Shork newShork = Instantiate(
                shorkPrefab, //Our shork prefab
                Random.insideUnitSphere * flockCount * feeshDensity,  //A random position in sphere (with size based on number of feesh) to spawn it at
                Random.rotation, //Facing a random direction
                transform //With this class as the parent
            );
            newShork.name = "shork " + i;
            shorks.Add(newShork);
        }
    }

    void Update()
    {
        foreach (Shork shork in shorks)
        { //Oh god, a loop for for every single frame.
            List<Transform> nearby = GetNearbyObjects(shork); //List of everything near current feesh
            Vector3 direction = behavior.CalculateDirection(shork, nearby); //Calculate our direction
            direction *= velocityMultiplier;
            if (direction.sqrMagnitude > squareMaxVelocity)
            { //If greater than max velocity
                direction = direction.normalized * maxVelocity; //Normalize (set it to 1) and set to max speed
            }
            shork.Move(direction); //Move in direction
        }
    }

    List<Transform> GetNearbyObjects(Shork shork)
    {
        List<Transform> nearby = new List<Transform>(); //List of nearby transforms.
        int nearFishCount = 0;
        Collider[] nearbyNeighborColliders = Physics.OverlapSphere(shork.transform.position, neighborRadius); //Array of nearby colliders around current fish within neighbor radius
        Collider[] nearbyObstacleColliders = Physics.OverlapSphere(shork.transform.position, obstacleAvoidanceRadius); //Array of nearby colliders within obstacle avoidance readius
        foreach (Collider collider in nearbyNeighborColliders)
        { //Another loop being ran inside a loop for every frame. I do not like this.
            if (!collider.gameObject.layer.Equals(LayerMask.NameToLayer("Obstacle"))) //If not an obstacle
            {
                if (collider != shork.getFeeshCollider)
                { //If collider is not feesh's own collider 
                    nearFishCount++;
                    nearby.Add(collider.transform); //Add transform of nearby collider to nearby list and follow anyways
                }
            }
        }
        shork.NearbyCount = nearFishCount;
        foreach (Collider collider in nearbyObstacleColliders)
        {
            if (collider.gameObject.layer.Equals(LayerMask.NameToLayer("Obstacle"))) { //If an obstalce
                nearby.Add(collider.transform); //Add collider no matter what
            }
        }

        return nearby;
    }



}

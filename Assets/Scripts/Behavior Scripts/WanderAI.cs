using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderAI : MonoBehaviour {

    public float MoveSpeed = 5f;
    public float RotSpeed = 50f;

    private bool isWandering;
    private bool isRotatingLeft;
    private bool isRotatingRight;
    private bool isRotatingUp;
    private bool isRotatingDown;
    private bool isWalking;
    Vector3 currentVelocity;
    void Start() 
    {
        this.isWandering = false;
        this.isRotatingLeft = false;
        this.isRotatingRight = false;
        this.isRotatingUp = false;
        this.isRotatingDown = false;
        this.isWalking = false;
    }

    void Update()
    {
        if (!isWandering) {
            StartCoroutine(Wander());
        }

        if (isRotatingRight) {
            transform.Rotate(transform.up * RotSpeed * Time.deltaTime);
        } else if (isRotatingLeft) {
            transform.Rotate(transform.up * -RotSpeed * Time.deltaTime);
        }
        
        if (isRotatingUp) {
            transform.Rotate(transform.right * RotSpeed * Time.deltaTime);
        } else if (isRotatingDown) {
            transform.Rotate(transform.right * -RotSpeed * Time.deltaTime);
        }

        if (isWalking) {
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
            List<Transform> obstacles = GetNearbyObjects();
            Vector3 avoidanceDirection = LeaderAvoidObstacles(obstacles);
            if(!avoidanceDirection.Equals(Vector3.zero)) { //If we need to avoid something
                transform.forward = LeaderAvoidObstacles(obstacles); //Calculate avoidance movement
                transform.position += transform.forward * Time.deltaTime;
            }
        }

    }

    IEnumerator Wander()
    {
        bool rotateLoR = (Random.value > 0.5f);
        bool rotateUoD = (Random.value > 0.5f);
        float rotTime = Random.Range(0.1f, 0.4f);
        float rotWait = Random.Range(0, 1);
        float walkWait = Random.Range(0, 1);
        float walkTime = Random.Range(1, 3);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotWait);

        isRotatingLeft = rotateLoR;
        isRotatingRight = !rotateLoR;
        isRotatingUp = rotateUoD;
        isRotatingDown = !rotateUoD;
        yield return new WaitForSeconds(rotTime);
        isRotatingLeft = false;
        isRotatingRight = false;
        isRotatingUp = false;
        isRotatingDown = false;

        isWandering = false;
    }

    List<Transform> GetNearbyObjects()
    {
        List<Transform> nearby = new List<Transform>(); //List of nearby transforms.
        Collider[] nearbyNeighborColliders = Physics.OverlapSphere(this.transform.position, 5f); //Array of nearby colliders around leader within obstacle radius
        foreach (Collider collider in nearbyNeighborColliders)
        {
            if (collider.gameObject.layer.Equals(LayerMask.NameToLayer("Obstacle")))
            { //If an obstalce
                nearby.Add(collider.transform); //Add collider no matter what
            }
        }


        return nearby;
    }

    public Vector3 LeaderAvoidObstacles(List<Transform> nearbyObstacles) {
        if (nearbyObstacles.Count == 0)
        { //If nothing nearby
            return Vector3.zero; //No adjustment necessary
        }
        Vector3 avoidanceDirection = Vector3.zero; //Direction to move to avoid obstacles
        float biggestThreatDistance = float.MaxValue;
        foreach (Transform nearby in nearbyObstacles)
        {
            if (nearby.gameObject.layer.Equals(LayerMask.NameToLayer("Obstacle"))) //If nearby is an obstacle
            {

                float distanceToObject = Vector3.Distance(nearby.transform.position, this.transform.position);
                if (distanceToObject < biggestThreatDistance)
                {
                    biggestThreatDistance = distanceToObject;
                    avoidanceDirection = this.transform.position - nearby.position; //Current position minus position of obstacle feesh. This also calculates offset from global position  
                }

            }
        }
        avoidanceDirection = Vector3.SmoothDamp(this.transform.forward, avoidanceDirection, ref currentVelocity, 1f);

        return avoidanceDirection * 0.05f;

    }
}
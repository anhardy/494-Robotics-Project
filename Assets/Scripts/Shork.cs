using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))] //Needed to see what's around the feesh
public class Shork : MonoBehaviour
{
    Collider feeshCollider;
    int nearbyCount = 0;
    public Collider getFeeshCollider {
        get {
            return feeshCollider;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        feeshCollider = GetComponent<Collider>();
    }

    public int NearbyCount { 
        get {
            return nearbyCount;
            }
        set {
            nearbyCount = value;
        }
    }

    public void Move(Vector3 velocity) {
        transform.forward = velocity;
        transform.position += velocity * Time.deltaTime; //Add our velocity to our current position (framerate independent)
    }
}
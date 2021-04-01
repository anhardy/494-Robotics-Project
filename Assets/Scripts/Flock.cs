using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))] //Needed to see what's around the feesh
public class Flock : MonoBehaviour
{
    Collider feeshCollider;
    public Collider FeeshCollider {
        get {
            return feeshCollider;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        feeshCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

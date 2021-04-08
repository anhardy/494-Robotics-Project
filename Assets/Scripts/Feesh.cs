using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))] //Needed to see what's around the feesh
public class Feesh : MonoBehaviour
{
    Flock thisFlock; //The Flock object/class this feesh belongs to
    Collider feeshCollider;
    public Collider getFeeshCollider {
        get {
            return feeshCollider;
        }
    }
    public Flock getFlock {
        get {
            return thisFlock;
        }
    }
    public void InitializeFlock(Flock flock) {
        thisFlock = flock;
    }
    // Start is called before the first frame update
    void Start()
    {
        feeshCollider = GetComponent<Collider>();
    }

    public void Move(Vector3 velocity) {
        transform.forward = velocity;
        transform.position += velocity * Time.deltaTime; //Add our velocity to our current position (framerate independent)
    }
}

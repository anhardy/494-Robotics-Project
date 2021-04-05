using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour {
    public static BoidManager instance = null;
    private List<Object> boids;
    public GameObject feesh;

    // BOUNDARIES
    public const float MAX_X = 50f;
    public const float MAX_Y = 50f;
    public const float MAX_Z = 50f;
    private const int BOID_COUNT = 100;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);    

        init();
    }

    void init() {
        boids = new List<Object>();
        System.Random rand = new System.Random();
        for (int i = 0; i < BOID_COUNT; i++) {
            var rotation = Quaternion.Euler (0f, (float)rand.Next(361), (float)rand.Next(361));
            
            float x = (float)(rand.NextDouble() * MAX_X * 2 - MAX_X);
            float y = (float)(rand.NextDouble() * MAX_Y);
            float z = (float)(rand.NextDouble() * MAX_Z * 2 - MAX_Z);
            boids.Add(Instantiate(feesh, new Vector3(x, y, z), rotation));
        }
    }
}
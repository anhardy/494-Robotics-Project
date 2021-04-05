using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private const float MAX_THETA = 0.1f;
    private float speed = 0.02f;
    private float thetaY = 0f;
    private float thetaZ = 0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        wander();
        transform.Translate (speed, 0f, 0.000f);  
        transform.Rotate(0.0f, thetaY, thetaZ);
    }

    void wander()
    {
        System.Random rand = new System.Random();
        float thetaStep = MAX_THETA / 10f;
        float deltaThetaY = 0f;
        float deltaThetaZ = 0f;

        if (rand.Next(2) == 0)
            deltaThetaY = thetaStep;
        else
            deltaThetaY = -thetaStep;

        if (rand.Next(2) == 0)
            deltaThetaZ = thetaStep;
        else
            deltaThetaZ = -thetaStep;

        
        thetaY += deltaThetaY;
        thetaZ += deltaThetaZ;

        if (thetaY < -MAX_THETA || thetaY > MAX_THETA)
            thetaY -= (deltaThetaY * 2);

        if (thetaZ < -MAX_THETA || thetaZ > MAX_THETA)
            thetaZ -= (deltaThetaZ * 2);
    }
}

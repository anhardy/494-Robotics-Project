using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color (0.22f, 0.65f, 0.77f, 0.5f);
        RenderSettings.fogDensity = 0.025f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

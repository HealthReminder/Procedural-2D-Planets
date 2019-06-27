using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int planetSeed;
    public Planet planet;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
            PlanetGenerator.instance.GeneratePlateTectonics(planet,planetSeed);
        if(Input.GetKeyDown(KeyCode.Y))
            PlanetGenerator.instance.GeneratePlateTectonics(planet,planetSeed);
    }
}

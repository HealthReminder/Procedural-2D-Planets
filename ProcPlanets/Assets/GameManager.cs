using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Planet planet;
    public bool isUsingSeed;
    [Range(0,100)] public int randomSeedInt;
    [Range(0,100)] public int transformSeedInt;
    [Range(0,100)] public int tectonicsSeedInt;
    [Range(0,100)] public int oceanSeedInt;
    [Range(0,100)] public int colorSeedInt;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)){
            
            if(!isUsingSeed) {
                randomSeedInt = Random.Range(0,100);
                transformSeedInt = Random.Range(0,100);
                tectonicsSeedInt = Random.Range(0,100);
                oceanSeedInt = Random.Range(30,150 - tectonicsSeedInt);
                colorSeedInt = Random.Range(0,100);
            }
            float randomSeed = (float)randomSeedInt/100;
            float transformSeed = (float)transformSeedInt/100;
            float tectonicsSeed = (float)tectonicsSeedInt/100;
            float oceanSeed = (float)oceanSeedInt/100;   
            float colorSeed = (float)colorSeedInt/100;
            Debug.Log("Generating planet with the following seed: "+randomSeedInt+""+transformSeedInt+""+tectonicsSeedInt+""+oceanSeedInt+""+colorSeed);
            PlanetGenerator.instance.GeneratePlateTectonics(planet,randomSeed,transformSeed,tectonicsSeed,oceanSeed,colorSeed);
        }
    }
}

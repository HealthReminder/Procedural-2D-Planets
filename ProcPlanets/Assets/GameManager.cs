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
    int colorSeedInt;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)){
            
            if(!isUsingSeed) {
                //randomSeedInt = Random.Range(0,1);
                //transformSeedInt = Random.Range(0.2f,0.8f);
               //tectonicsSeedInt = Random.Range(0,1);
               // oceanSeedInt = Random.Range(0.3f,1.5f - tectonicsSeedInt);
            }
            float randomSeed = Random.Range(0f,1f);
            float transformSeed = Random.Range(0.2f,0.8f);
            float tectonicsSeed = Random.Range(0f,1f);
            float oceanSeed = Random.Range(0.3f,1.5f - tectonicsSeedInt);
            float colorSeed = 1-tectonicsSeed;
            /*float randomSeed = (float)randomSeedInt/100;
            float transformSeed = (float)transformSeedInt/100;
            float tectonicsSeed = (float)tectonicsSeedInt/100;
            float oceanSeed = (float)oceanSeedInt/100;   
            float colorSeed = 1-tectonicsSeed; */
            Debug.Log("Generating planet with the following seed: "+randomSeedInt+""+transformSeedInt+""+tectonicsSeedInt+""+oceanSeedInt+""+colorSeed);
            PlanetGenerator.instance.GeneratePlateTectonics(planet,randomSeed,transformSeed,tectonicsSeed,oceanSeed,colorSeed);
        }
    }
}

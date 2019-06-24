using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    public float initialBoneDistanceFromCore = 1;
    public float heightDifferenceFromPlateType = 1;
    public static PlanetGenerator instance;
    private void Awake() {
        instance = this;
    }

    public void GeneratePlateTectonics(Planet planet,int seed) {
        StartCoroutine(GeneratePlateTectonicsRoutine(planet,seed));
    }
    IEnumerator GeneratePlateTectonicsRoutine(Planet planet,int seed){
        Debug.Log("TECTONICS - Initiated");

        LineTest.instance.DisableLines();

        float randomSeed = Random.Range(0,1f);
        float rotationSeed = Random.Range(0f,1f);
        float tectonicsSeed = Random.Range(0f,1f);
        Debug.Log(randomSeed+"A"+rotationSeed+"B"+tectonicsSeed+"C");
        Random.InitState((int)Mathf.Lerp(0,999999,randomSeed));
        yield return null;
        Debug.Log("TECTONICS - Generated variables");
            

    
        PlanetView.instance.Clear();
        planet.enabled = false;
        Transform[] planetBones = planet.bones;
        for (int currentBone = 0; currentBone < planetBones.Length; currentBone++)
            planetBones[currentBone].position = planet.transform.position + (planetBones[currentBone].right*10*-initialBoneDistanceFromCore);
        
        yield return null;
        Debug.Log("TECTONICS - Reseted bone positions and collider");



        planet.transform.Rotate(new Vector3(0,0,Mathf.Lerp(0,360,rotationSeed)));
        Debug.Log("TECTONICS - Assigned initial random rotation");



        int plateQuantity = (int)Mathf.Lerp(2,12,tectonicsSeed);
        planet.plateQuantity = plateQuantity;
        Debug.Log("TECTONICS - Set plate quantity to "+plateQuantity);


        PlateTectonic[] newPlates = new PlateTectonic[plateQuantity];
        for (int currentPlate = 0; currentPlate < newPlates.Length; currentPlate++)
        {
            newPlates[currentPlate] = new PlateTectonic();
            newPlates[currentPlate].bones = new List<PlateBone>();
        }
        yield return null;
        Debug.Log("TECTONICS - Reseted plates");



        for (int currentBone = 0; currentBone < planetBones.Length; currentBone++)
        {
            float percentage = Mathf.InverseLerp(0,planetBones.Length-1, currentBone);
            int identification = (int)Mathf.Lerp(0,plateQuantity-1, percentage);
            PlateBone newBone = new PlateBone();
            //newBone.pressure = 0;
            newBone.translation = Vector3.zero;
            newBone.transform = planetBones[currentBone];
            newPlates[identification].bones.Add(newBone);
        }
        planet.plates = newPlates;
        yield return null;
        Debug.Log("TECTONICS - Assigned reseted bones to plates");



        for (int currentPlate = 0; currentPlate < newPlates.Length; currentPlate++){
            int newType = Random.Range(0,2);
            newPlates[currentPlate].type = newType;
            if(newType == 0){
                newPlates[currentPlate].weightIndex = (Random.Range(6,10));
            }else if(newType == 1){
                newPlates[currentPlate].weightIndex = (Random.Range(1,5));
            }newPlates[currentPlate].movementForce = 0;
        }
        planet.plates = newPlates;
        yield return null;
        Debug.Log("TECTONICS - Assigned types, weights and initial movement force to plates");

        

        //float heightDifferenceFromPlateType = Random.Range(1,3);
        for (int currentPlateIndex = 0; currentPlateIndex < newPlates.Length; currentPlateIndex++)
        {
            PlateTectonic currentPlate = newPlates[currentPlateIndex];
            PlateBone currentBone;
            if(currentPlate.type == 1)
                for (int currentBoneIndex = 0; currentBoneIndex < currentPlate.bones.Count; currentBoneIndex++)
                {
                    currentBone = currentPlate.bones[currentBoneIndex];
                    currentBone.transform.position += -currentBone.transform.right * heightDifferenceFromPlateType;
                    currentBone.translation += -currentBone.transform.right * heightDifferenceFromPlateType;
                }
            yield return null;
        }
        planet.plates = newPlates;
        yield return null;
        Debug.Log("TECTONICS - Set initial plate positions");

        

        int tectonicActivity = (int)(Mathf.Lerp(0,4,tectonicsSeed));
        int tectonicPoints = (int)(Mathf.Lerp(tectonicActivity,planet.plateQuantity,tectonicsSeed));
        for (int currentPoint = 0; currentPoint < tectonicPoints; currentPoint++)
        {
            
            //Get initial impact index
            int initialPointIndex = Random.Range(0,newPlates.Length);
            //Find direction using a coin flip and the activity
            int coinFlip = Random.Range(0,2);
            float initialForce;
            if(coinFlip == 0)
                initialForce = tectonicActivity;
            else 
                initialForce = -tectonicActivity;

            newPlates[initialPointIndex].movementForce = initialForce;

            if(initialForce < 0){
                int currentPlateIndex = initialPointIndex -1;
                for (float currentForce = initialForce-1; currentForce < 0; currentForce++)
                {
                    if(currentPlateIndex >= newPlates.Length)
                        currentPlateIndex = 0;
                    else if(currentPlateIndex < 0)
                        currentPlateIndex = newPlates.Length-1;

                    newPlates[currentPlateIndex].movementForce += currentForce;
                    Mathf.Clamp(newPlates[currentPlateIndex].movementForce,-3,4);
                    Debug.Log("Set force of "+currentForce+" to plate "+currentPlateIndex+" from initial force of "+initialForce);
                    currentPlateIndex--;
                }
            } else {
                int currentPlateIndex = initialPointIndex +1;
                for (float currentForce = initialForce-1; currentForce > 0; currentForce--)
                {
                    if(currentPlateIndex >= newPlates.Length)
                        currentPlateIndex = 0;
                    else if(currentPlateIndex < 0)
                        currentPlateIndex = newPlates.Length-1;

                    newPlates[currentPlateIndex].movementForce += currentForce;
                    Mathf.Clamp(newPlates[currentPlateIndex].movementForce,-3,4);
                    currentPlateIndex++;
                }
            }
            yield return null;    
        }
        planet.tectonicPointsQuantity = tectonicPoints;
        planet.plates = newPlates;
        Debug.Log("TECTONICS - Distributed plate flows");


        PlateTectonic leftPlate;
        PlateTectonic rightPlate;
        for (int currentPlate = 0; currentPlate < newPlates.Length; currentPlate++)
        {
            //Get victim (always the plate on the right)
            int victimIndex = currentPlate+1;
            if(victimIndex >= newPlates.Length)
                victimIndex = 0;

            leftPlate = newPlates[currentPlate];
            rightPlate = newPlates[victimIndex];
            
            //Identify the type of event that happened
            //string eventDescription = "";
            float eventForce = Mathf.Abs(leftPlate.movementForce - rightPlate.movementForce);
            if(leftPlate.movementForce <= 0){
                if(rightPlate.movementForce <= 0){
                    //This is a transform event
                    if(newPlates[currentPlate].weightIndex >= newPlates[victimIndex].weightIndex){
                        //The left plate is the heaviest
                        for (float iterationsLeft = eventForce/2; iterationsLeft > 0;)
                        {
                            if(iterationsLeft >= 1) {
                                MoveRandomBone(leftPlate.bones,1,-1);
                                MoveRandomBone(rightPlate.bones,-1,1); 
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomBone(leftPlate.bones,1,-iterationsLeft);
                                MoveRandomBone(rightPlate.bones,-1,iterationsLeft); 
                                iterationsLeft = -1;
                            }
                        }
                    } else {
                        //The right plate is the heaviest
                        for (float iterationsLeft = eventForce/2; iterationsLeft > 0;)
                        {
                            if(iterationsLeft >= 1) {
                                MoveRandomBone(rightPlate.bones,-1,-1);
                                MoveRandomBone(leftPlate.bones,1,1);
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomBone(leftPlate.bones,1,-iterationsLeft);
                                MoveRandomBone(rightPlate.bones,-1,iterationsLeft); 
                                iterationsLeft = -1;
                            }
                        }
                        
                    }
                } else {
                    //This is a divergence event
                    //The left plate is the heaviest
                    for (float iterationsLeft = eventForce; iterationsLeft > 0;)  {
                        if(iterationsLeft >= 1) {
                            MoveRandomBone(leftPlate.bones,1,-1);
                            MoveRandomBone(rightPlate.bones,-1,-1);
                            iterationsLeft -= 1;
                        } else {
                            MoveRandomBone(leftPlate.bones,1,-iterationsLeft);
                            MoveRandomBone(rightPlate.bones,-1,-iterationsLeft);
                            iterationsLeft = -1;
                        }
                    }
                    
                }
            } else  {
                if(rightPlate.movementForce <= 0){
                    //This is a convergence event
                    if(newPlates[currentPlate].weightIndex >= newPlates[victimIndex].weightIndex){
                        //The left plate is the heaviest
                        for (float iterationsLeft = eventForce; iterationsLeft > 0;)  {
                            if(iterationsLeft >= 1) {
                                MoveRandomBone(leftPlate.bones,1,-1);
                                MoveRandomBone(rightPlate.bones,-1,1);
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomBone(leftPlate.bones,1,-iterationsLeft);
                                MoveRandomBone(rightPlate.bones,-1,iterationsLeft);
                                iterationsLeft = -1;
                            }
                        }
                        
                    } else {
                        //The right plate is the heaviest
                        for (float iterationsLeft = eventForce; iterationsLeft > 0;)  {
                            if(iterationsLeft >= 1) {
                                MoveRandomBone(rightPlate.bones,-1,-1);
                                MoveRandomBone(leftPlate.bones,1,1);
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomBone(rightPlate.bones,-1,-iterationsLeft);
                                MoveRandomBone(leftPlate.bones,1,iterationsLeft);
                                iterationsLeft = -1;
                            }
                        }
                    }
                } else {
                    //This is a transform event
                    if(newPlates[currentPlate].weightIndex >= newPlates[victimIndex].weightIndex){
                        //The left plate is the heaviest
                        for (float iterationsLeft = eventForce/2; iterationsLeft > 0;)  {
                            if(iterationsLeft >= 1) {
                                MoveRandomBone(leftPlate.bones,1,-1);
                                MoveRandomBone(rightPlate.bones,-1,1);
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomBone(leftPlate.bones,1,-iterationsLeft);
                                MoveRandomBone(rightPlate.bones,-1,iterationsLeft);
                                iterationsLeft = -1;
                            }
                        }
                    } else {
                        //The right plate is the heaviest
                        for (float iterationsLeft = eventForce/2; iterationsLeft > 0;)  {
                            if(iterationsLeft >= 1) {
                                MoveRandomBone(rightPlate.bones,-1,-1);
                                MoveRandomBone(leftPlate.bones,1,1);
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomBone(rightPlate.bones,-1,-iterationsLeft);
                                MoveRandomBone(leftPlate.bones,1,iterationsLeft);
                                iterationsLeft = -1;
                            }
                        }
                    }
                }
            }
            yield return null;   
        }
        Debug.Log("TECTONICS - Ran early event interaction");



        



        Vector2[] newColliderPoints = new Vector2[planetBones.Length];
        for (int i = 0; i < newColliderPoints.Length; i++)
        {
            newColliderPoints[i] = planetBones[i].localPosition;
            yield return null;   
        }
        planet.collider.points = newColliderPoints;
        planet.enabled = true;
        yield return null;   
        Debug.Log("TECTONICS - Fixed collider");



        foreach(PlateTectonic currentPlate in planet.plates){
            AddPlateFeatures(currentPlate.bones,tectonicActivity/2);
            yield return null;
        }



        LineTest.instance.SetLines(planet.plates);
        yield return null;
        Debug.Log("TECTONICS - Line test");
        


        Debug.Log("TECTONICS - Ended");


        
        yield break;
    }
    
    void AddPlateFeatures(List<PlateBone> bones, int volcanicPoints) {
        //HERE YOU NEED TO GET THE LIST OF BONES ORDERED BY PRESSURE MAGNITUDE 
        //YOU MUST REALIZE THE VOLCANIC FEATURES USING THE VOLCANICPOINTS
        int placedVolcanos = 0;
        for (int currentBoneIndex = 0; currentBoneIndex < bones.Count; currentBoneIndex++)
        {
            if(placedVolcanos >= volcanicPoints)
                return;

            PlateBone bone = bones[currentBoneIndex];
            if(bone.pressure >= 1.35f){
                //Debug.Log("Spawned volcano on "+bone.transform.name);
                PlanetView.instance.SpawnVolcano(bone.transform);
            } else if(bone.pressure <= -1.8f){
                //Debug.Log("Spawned underwater volcano on "+bone.transform.name);
                PlanetView.instance.SpawnUnderwaterVolcano(bone.transform);
            }
            placedVolcanos++;
        }
    }

    void MoveRandomBone(List<PlateBone> bones,int direction, float amount) {
        int randomIndex = -1;
        if(direction == 1)
            randomIndex = Random.Range(bones.Count/2,bones.Count);
        else 
            randomIndex = Random.Range(0,bones.Count/2);

        PlateBone bone = bones[randomIndex];
        //Debug.Log(bone.transform.name+"   "+(-1*direction*amount) + "   "+bone.pressure+ "   "+(bone.pressure+(-1*direction*amount)));

        bone.transform.position += bone.transform.right*direction*amount;
        bone.pressure += -1*direction*amount;
        int coinFlip = Random.Range(0,2);
    }
}

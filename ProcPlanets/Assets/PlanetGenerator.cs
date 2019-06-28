using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    //public float initialBoneDistanceFromCore = 1;
    //public float heightDifferenceFromPlateType = 1;
    public static PlanetGenerator instance;
    public AnimationCurve medianCurve;
    private void Awake() {
        instance = this;
    }

    public void GeneratePlateTectonics(Planet planet,float randomSeed ,float transformSeed ,float tectonicsSeed ,float oceanSeed,float colorSeed) {
        StopCoroutine(GeneratePlateTectonicsRoutine(null,0,0,0,0,0));
        StartCoroutine(GeneratePlateTectonicsRoutine(planet,randomSeed,transformSeed,tectonicsSeed,oceanSeed,colorSeed));
    }
    IEnumerator GeneratePlateTectonicsRoutine(Planet planet ,float randomSeed ,float transformSeed ,float tectonicsSeed ,float oceanSeed,float colorSeed){
        Debug.Log("PLANET GENERATOR - Initiated");

        LineTest.instance.DisableLines();
        GlobalNotification.instance.Reset();
        yield return null;

        Debug.Log("PLANET GENERATOR - Aquired seeds");

        //tectonicsSeed = 1;
        //transformSeed = 1;
        //oceanSeed = 1;
        tectonicsSeed = medianCurve.Evaluate(tectonicsSeed);
        transformSeed = medianCurve.Evaluate(transformSeed);
        oceanSeed = medianCurve.Evaluate(oceanSeed);

        
        float randomColor;
        float initialBoneDistanceFromCore;
        int plateQuantity;
        int tectonicPoints;
        float heightDifferenceFromPlateType;
        float tectonicActivity;
        yield return null;
        Debug.Log("PLANET GENERATOR - Cached variables");


        
        Random.InitState((int)Mathf.Lerp(0,999999,randomSeed));
        randomColor = colorSeed;
        initialBoneDistanceFromCore = Mathf.Lerp(1,3,transformSeed);
        plateQuantity = (int)Mathf.Lerp(2,12,tectonicsSeed);
        tectonicActivity = Mathf.Lerp(0,3,tectonicsSeed)+0.5f;
        tectonicPoints = (int)(Mathf.Lerp(tectonicActivity,plateQuantity,tectonicsSeed));
        heightDifferenceFromPlateType = Mathf.Lerp(1,3, transformSeed);
        yield return null;
        Debug.Log("PLANET GENERATOR - Generated variables ");


        
        planet.data.tectonicPoints = tectonicPoints;
        planet.data.plateQuantity = plateQuantity;
        planet.data.heightDifferenceFromPlateType = heightDifferenceFromPlateType;
        planet.data.initialBoneDistanceFromCore = initialBoneDistanceFromCore;
        planet.data.tectonicActivity = tectonicActivity;
        planet.data.colorIndex = randomColor;
        Debug.Log("PLANET GENERATOR - Stored generated variables");



        //GlobalNotification.instance.PostNotification("Initial bone distance from core: <color=#ffff00ff>"+(int)(100*(initialBoneDistanceFromCore+1)/3)+"%</color>\n");
        //GlobalNotification.instance.PostNotification("Water percentage: <color=#ffff00ff>"+(int)(oceanSeed*100)+"%</color>\n");
        //GlobalNotification.instance.PostNotification("Plate tectonics quantity:  <color=#ffff00ff>"+plateQuantity+"</color>\n");
        //GlobalNotification.instance.PostNotification("Height difference from plate type: <color=#ffff00ff>"+(int)(100*(heightDifferenceFromPlateType+1)/3)+"%</color>\n");
        //Debug.Log("PLANET GENERATOR - Posted notifications");

        

        PlanetView.instance.ChangeColor(randomColor,randomColor,randomColor);
        yield return null;
        Debug.Log("PLANET GENERATOR - Changed colors");
            

    
        PlanetView.instance.Clear();
        planet.enabled = false;
        Transform[] planetBones = planet.bones;
        for (int currentBone = 0; currentBone < planetBones.Length; currentBone++)
            planetBones[currentBone].position = planet.transform.position + (planetBones[currentBone].right*10*-initialBoneDistanceFromCore);
        PlanetView.instance.ScaleOceans(Mathf.Lerp(1,3,transformSeed),oceanSeed);
        yield return null;
        Debug.Log("PLANET GENERATOR - Reseted bone positions and collider");
        


        planet.transform.Rotate(new Vector3(0,0,Mathf.Lerp(0,360,transformSeed)));
        Debug.Log("PLANET GENERATOR - Assigned initial random rotation");



        PlateTectonic[] newPlates = new PlateTectonic[plateQuantity];
        for (int currentPlate = 0; currentPlate < newPlates.Length; currentPlate++)
        {
            newPlates[currentPlate] = new PlateTectonic();
            newPlates[currentPlate].bones = new List<PlateBone>();
        }
        yield return null;
        Debug.Log("PLANET GENERATOR - Reseted plates");



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
        Debug.Log("PLANET GENERATOR - Assigned reseted bones to plates");



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
        Debug.Log("PLANET GENERATOR - Assigned types, weights and initial movement force to plates");

        

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
        Debug.Log("PLANET GENERATOR - Set initial plate positions");

        

        //GlobalNotification.instance.PostNotification("Tectonic activity: <color=#ffff00ff>"+(int)(100*tectonicActivity/3)+"%</color>\n");
        //GlobalNotification.instance.PostNotification("Tectonic points: <color=#ffff00ff>"+tectonicPoints+"</color>\n");
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
        
        planet.plates = newPlates;
        Debug.Log("PLANET GENERATOR - Distributed plate flows");


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
                                MoveRandomBone(leftPlate.bones,1,-1,transformSeed);
                                MoveRandomBone(rightPlate.bones,-1,1,transformSeed); 
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomBone(leftPlate.bones,1,-iterationsLeft,transformSeed);
                                MoveRandomBone(rightPlate.bones,-1,iterationsLeft,transformSeed); 
                                iterationsLeft = -1;
                            }
                        }
                    } else {
                        //The right plate is the heaviest
                        for (float iterationsLeft = eventForce/2; iterationsLeft > 0;)
                        {
                            if(iterationsLeft >= 1) {
                                MoveRandomBone(rightPlate.bones,-1,-1,transformSeed);
                                MoveRandomBone(leftPlate.bones,1,1,transformSeed);
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomBone(leftPlate.bones,1,-iterationsLeft,transformSeed);
                                MoveRandomBone(rightPlate.bones,-1,iterationsLeft,transformSeed); 
                                iterationsLeft = -1;
                            }
                        }
                        
                    }
                } else {
                    //This is a divergence event
                    //The left plate is the heaviest
                    for (float iterationsLeft = eventForce; iterationsLeft > 0;)  {
                        if(iterationsLeft >= 1) {
                            MoveRandomBone(leftPlate.bones,1,-1,transformSeed);
                            MoveRandomBone(rightPlate.bones,-1,-1,transformSeed);
                            iterationsLeft -= 1;
                        } else {
                            MoveRandomBone(leftPlate.bones,1,-iterationsLeft,transformSeed);
                            MoveRandomBone(rightPlate.bones,-1,-iterationsLeft,transformSeed);
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
                                MoveRandomBone(leftPlate.bones,1,-1,transformSeed);
                                MoveRandomBone(rightPlate.bones,-1,1,transformSeed);
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomBone(leftPlate.bones,1,-iterationsLeft,transformSeed);
                                MoveRandomBone(rightPlate.bones,-1,iterationsLeft,transformSeed);
                                iterationsLeft = -1;
                            }
                        }
                        
                    } else {
                        //The right plate is the heaviest
                        for (float iterationsLeft = eventForce; iterationsLeft > 0;)  {
                            if(iterationsLeft >= 1) {
                                MoveRandomBone(rightPlate.bones,-1,-1,transformSeed);
                                MoveRandomBone(leftPlate.bones,1,1,transformSeed);
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomBone(rightPlate.bones,-1,-iterationsLeft,transformSeed);
                                MoveRandomBone(leftPlate.bones,1,iterationsLeft,transformSeed);
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
                                MoveRandomBone(leftPlate.bones,1,-1,transformSeed);
                                MoveRandomBone(rightPlate.bones,-1,1,transformSeed);
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomBone(leftPlate.bones,1,-iterationsLeft,transformSeed);
                                MoveRandomBone(rightPlate.bones,-1,iterationsLeft,transformSeed);
                                iterationsLeft = -1;
                            }
                        }
                    } else {
                        //The right plate is the heaviest
                        for (float iterationsLeft = eventForce/2; iterationsLeft > 0;)  {
                            if(iterationsLeft >= 1) {
                                MoveRandomBone(rightPlate.bones,-1,-1,transformSeed);
                                MoveRandomBone(leftPlate.bones,1,1,transformSeed);
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomBone(rightPlate.bones,-1,-iterationsLeft,transformSeed);
                                MoveRandomBone(leftPlate.bones,1,iterationsLeft,transformSeed);
                                iterationsLeft = -1;
                            }
                        }
                    }
                }
            }
            yield return null;   
        }
        Debug.Log("PLANET GENERATOR - Ran early event interaction");



        Vector2[] newColliderPoints = new Vector2[planetBones.Length];
        for (int i = 0; i < newColliderPoints.Length; i++)
        {
            newColliderPoints[i] = planetBones[i].localPosition;
            yield return null;   
        }
        planet.collider.points = newColliderPoints;
        planet.enabled = true;
        yield return null;   
        Debug.Log("PLANET GENERATOR - Fixed collider");



        foreach(PlateTectonic currentPlate in planet.plates){
            AddPlateFeatures(currentPlate.bones,1+((int)tectonicActivity/2));
            yield return null;
        }



        LineTest.instance.SetLines(planet.plates);
        yield return null;
        Debug.Log("PLANET GENERATOR - Line test");
        


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

    void MoveRandomBone(List<PlateBone> bones,int direction, float amount, float transformSeed) {
        int randomIndex = -1;
        PlateBone bone;
        if(direction == 1){
            randomIndex = Random.Range(bones.Count/2,bones.Count);
            bone = bones[randomIndex];
            if(bone.pressure >= 3) 
                return;
        } else {
            randomIndex = Random.Range(0,bones.Count/2);
            bone = bones[randomIndex];
            if(bone.pressure <= -2.5f) 
                return;
        }

        //Debug.Log(bone.transform.name+"   "+(-1*direction*amount) + "   "+bone.pressure+ "   "+(bone.pressure+(-1*direction*amount)));
        float heightFromTransformSeed = Mathf.Lerp(0.3f,1f,transformSeed);
        bone.transform.position += bone.transform.right*direction*amount*heightFromTransformSeed;
        bone.pressure += -1*direction*amount*heightFromTransformSeed;
        
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGenerator : MonoBehaviour
{
    public static GlobalGenerator instance;
    private void Awake() {
        instance = this;
    }

    public void GeneratePlateTectonics(Planet planet) {
        StartCoroutine(GeneratePlateTectonicsRoutine(planet));
    }
    IEnumerator GeneratePlateTectonicsRoutine(Planet planet){
        Debug.Log("TECTONICS - Initiated");



        Transform[] planetBones = planet.bones;
        for (int currentBone = 0; currentBone < planetBones.Length; currentBone++)
        {
            planetBones[currentBone].position = new Vector3(0,0,0);
        }
        yield return null;
        Debug.Log("TECTONICS - Reseted bone positions");


        planet.transform.Rotate(new Vector3(0,0,Random.Range(0,361)));
        Debug.Log("TECTONICS - Assigned initial random rotation");



        int plateQuantity = Random.Range(2,7);
        planet.plateQuantity = plateQuantity;
        Debug.Log("TECTONICS - Set plate quantity to "+plateQuantity);


        PlateTectonic[] newPlates = new PlateTectonic[plateQuantity];
        for (int currentPlate = 0; currentPlate < newPlates.Length; currentPlate++)
        {
            newPlates[currentPlate] = new PlateTectonic();
            newPlates[currentPlate].bones = new List<Transform>();
        }
        yield return null;
        Debug.Log("TECTONICS - Reseted plates");



        for (int currentBone = 0; currentBone < planetBones.Length; currentBone++)
        {
            float percentage = Mathf.InverseLerp(0,planetBones.Length-1, currentBone);
            int identification = (int)Mathf.Lerp(0,plateQuantity-1, percentage);
            newPlates[identification].bones.Add(planetBones[currentBone]);
        }
        yield return null;
        planet.plates = newPlates;
        Debug.Log("TECTONICS - Assigned bones to plates");



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
        Debug.Log("TECTONICS - Assigned types, weights and initial movement force to plates");

        

        for (int currentPlate = 0; currentPlate < newPlates.Length; currentPlate++)
        {
            float defaultPlateHeight;
            if(newPlates[currentPlate].type == 0)
                defaultPlateHeight = Random.Range(1,1.8f);
            else
                defaultPlateHeight = Random.Range(2.2f,3f);
            newPlates[currentPlate].defaultHeight = defaultPlateHeight;
            Transform currentBoneT;
            for (int currentBone = 0; currentBone < newPlates[currentPlate].bones.Count; currentBone++)
            {
                currentBoneT = newPlates[currentPlate].bones[currentBone].transform; 
                currentBoneT.position = currentBoneT.right * defaultPlateHeight;
            }
            yield return null;
        }
        planet.plates = newPlates;
        Debug.Log("TECTONICS - Set initial plate positions");


        int tectonicPoints = Random.Range(1,planet.plateQuantity);
        int tectonicActivity = Random.Range(1,planet.plateQuantity/2);
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
                    Debug.Log("Set force of "+currentForce+" to plate "+currentPlateIndex+" from initial force of "+initialForce);
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
            //if(eventForce <= -3 || eventForce >= 4)
                //PlanetView.instance.SpawnVolcano(rightPlate.)
            if(leftPlate.movementForce <= 0){
                if(rightPlate.movementForce <= 0){
                    //This is a transform event
                    if(newPlates[currentPlate].weightIndex >= newPlates[victimIndex].weightIndex){
                        //The left plate is the heaviest
                        for (float iterationsLeft = eventForce/2; iterationsLeft > 0;)
                        {
                            if(iterationsLeft >= 1) {
                                MoveRandomTransformRight(leftPlate.bones,1,-1);
                                MoveRandomTransformRight(rightPlate.bones,-1,1); 
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomTransformRight(leftPlate.bones,1,-iterationsLeft);
                                MoveRandomTransformRight(rightPlate.bones,-1,iterationsLeft); 
                                iterationsLeft = -1;
                            }
                            yield return null;
                        }
                    } else {
                        //The right plate is the heaviest
                        for (float iterationsLeft = eventForce/2; iterationsLeft > 0;)
                        {
                            if(iterationsLeft >= 1) {
                                MoveRandomTransformRight(rightPlate.bones,-1,-1);
                                MoveRandomTransformRight(leftPlate.bones,1,1);
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomTransformRight(leftPlate.bones,1,-iterationsLeft);
                                MoveRandomTransformRight(rightPlate.bones,-1,iterationsLeft); 
                                iterationsLeft = -1;
                            }
                            yield return null;
                        }
                        
                    }
                } else {
                    //This is a divergence event
                    //The left plate is the heaviest
                    for (float iterationsLeft = eventForce; iterationsLeft > 0;)  {
                        if(iterationsLeft >= 1) {
                            MoveRandomTransformRight(leftPlate.bones,1,-1);
                            MoveRandomTransformRight(rightPlate.bones,-1,-1);
                            iterationsLeft -= 1;
                        } else {
                            MoveRandomTransformRight(leftPlate.bones,1,-iterationsLeft);
                            MoveRandomTransformRight(rightPlate.bones,-1,-iterationsLeft);
                            iterationsLeft = -1;
                        }
                        yield return null;
                    }
                    
                }
            } else  {
                if(rightPlate.movementForce <= 0){
                    //This is a convergence event
                    if(newPlates[currentPlate].weightIndex >= newPlates[victimIndex].weightIndex){
                        //The left plate is the heaviest
                        for (float iterationsLeft = eventForce; iterationsLeft > 0;)  {
                            if(iterationsLeft >= 1) {
                                MoveRandomTransformRight(leftPlate.bones,1,-1);
                                MoveRandomTransformRight(rightPlate.bones,-1,1);
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomTransformRight(leftPlate.bones,1,-iterationsLeft);
                                MoveRandomTransformRight(rightPlate.bones,-1,iterationsLeft);
                                iterationsLeft = -1;
                            }
                        yield return null;
                        }
                        
                    } else {
                        //The right plate is the heaviest
                        for (float iterationsLeft = eventForce; iterationsLeft > 0;)  {
                            if(iterationsLeft >= 1) {
                                MoveRandomTransformRight(rightPlate.bones,-1,-1);
                                MoveRandomTransformRight(leftPlate.bones,1,1);
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomTransformRight(rightPlate.bones,-1,-iterationsLeft);
                                MoveRandomTransformRight(leftPlate.bones,1,iterationsLeft);
                                iterationsLeft = -1;
                            }
                        yield return null;
                        }
                    }
                } else {
                    //This is a transform event
                    if(newPlates[currentPlate].weightIndex >= newPlates[victimIndex].weightIndex){
                        //The left plate is the heaviest
                        for (float iterationsLeft = eventForce/2; iterationsLeft > 0;)  {
                            if(iterationsLeft >= 1) {
                                MoveRandomTransformRight(leftPlate.bones,1,-1);
                                MoveRandomTransformRight(rightPlate.bones,-1,1);
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomTransformRight(leftPlate.bones,1,-iterationsLeft);
                                MoveRandomTransformRight(rightPlate.bones,-1,iterationsLeft);
                                iterationsLeft = -1;
                            }
                        yield return null;
                        }
                    } else {
                        //The right plate is the heaviest
                        for (float iterationsLeft = eventForce/2; iterationsLeft > 0;)  {
                            if(iterationsLeft >= 1) {
                                MoveRandomTransformRight(rightPlate.bones,-1,-1);
                                MoveRandomTransformRight(leftPlate.bones,1,1);
                                iterationsLeft -= 1;
                            } else {
                                MoveRandomTransformRight(rightPlate.bones,-1,-iterationsLeft);
                                MoveRandomTransformRight(leftPlate.bones,1,iterationsLeft);
                                iterationsLeft = -1;
                            }
                        yield return null;
                        }
                    }
                }
            }
            
            /*
            if(attackForce > 0){

                for(float eventQuantity = newPlates[currentPlate].movementForce; eventQuantity > 0;){
                    if(eventQuantity < 1)
                        MoveRandomTransformRight(newPlates[currentPlate].bones,1,eventQuantity);
                    else
                        MoveRandomTransformRight(newPlates[currentPlate].bones,1,0.8f);
                        
                    eventQuantity--;
                }
                
            } else {
                for(float eventQuantity = -1*newPlates[currentPlate].movementForce; eventQuantity > 0;){
                    if(eventQuantity < 1)
                        MoveRandomTransformRight(newPlates[currentPlate].bones,-1,eventQuantity);
                    else
                        MoveRandomTransformRight(newPlates[currentPlate].bones,-1,0.5f);
                    eventQuantity--;
                }
            } */
            yield return null;   
        }



        Debug.Log("TECTONICS - Ran early event interaction");


        Debug.Log("TECTONICS - Ended");


        
        yield break;
    }

    void MoveRandomTransformRight(List<Transform> bones,int direction, float amount) {
        int randomIndex = -1;
        if(direction == 1)
            randomIndex = Random.Range(bones.Count/2,bones.Count);
        else 
            randomIndex = Random.Range(0,bones.Count/2);

        Transform bone = bones[randomIndex];
        bone.position += bone.right*direction*amount;
    }
}

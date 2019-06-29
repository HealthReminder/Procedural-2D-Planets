﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
   public class Planet : MonoBehaviour
{
    public PlanetData data;
    public new PolygonCollider2D collider;
    public Transform[]  bones;
    private void Awake() {
        data = new PlanetData();
    }
}
public class PlanetData {
    [Header("Tectonics")]
    public float initialBoneDistanceFromCore; //initialBoneDistanceFromCore
    public int plateQuantity;
    public int tectonicPoints;
    public float heightDifferenceFromPlateType;
    public float tectonicActivity;
    [Header("Color")]
    public float colorIndex;
    [SerializeField]  public PlateTectonic[] plates;
}

[System.Serializable] public class PlateTectonic {
    public int type;
    public int weightIndex;
    public float movementForce;
    public List<PlateBone> bones;
    
}
[System.Serializable] public class PlateBone {
    public Transform transform;
    public float pressure;
    public Vector3 translation;
}


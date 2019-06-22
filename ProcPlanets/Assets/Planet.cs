using System.Collections;
using System.Collections.Generic;
using UnityEngine;
   public class Planet : MonoBehaviour
{
    public Transform[]  bones;
    [Header("Plate tectonics")]
    public int plateQuantity;
    [SerializeField]  public PlateTectonic[] plates;
    public int tectonicPointsQuantity;
    
}

[System.Serializable] public class PlateTectonic {
    public int type;
    public int weightIndex;
    public float movementForce;
    public List<Transform> bones;
    
}
[System.Serializable] public class PlateBone {
    public Transform transform;
    public float pressure;
    public float height;
}


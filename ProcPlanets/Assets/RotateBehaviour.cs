using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBehaviour : MonoBehaviour
{
    public float zRotation;
    private void Update() {
        transform.Rotate(new Vector3(0,0,zRotation));
    }
}

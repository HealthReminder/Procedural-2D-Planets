using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float force = 10;
    public Rigidbody2D rigidBody;
    private void Update() {
        if(Input.GetAxis("Vertical") != 0)
            rigidBody.AddRelativeForce(new Vector2(0,Input.GetAxis("Vertical")*force));
        if(Input.GetAxis("Horizontal") != 0)
            rigidBody.AddRelativeForce(new Vector2(Input.GetAxis("Horizontal")*force,0));
    }
}

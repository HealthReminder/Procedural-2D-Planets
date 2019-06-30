using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBehaviour : MonoBehaviour
{
    public bool isActiveOnStart = false;
    public GameObject target;
    void Start()
    {
        if(isActiveOnStart)
            target.SetActive(true);
    }
    public void DelayedDeactivate(float delay){
        Invoke("Deactivate",delay);
    }
    void Deactivate(){
        target.SetActive(false);
    }
    public void DelayedActivate(float delay){
        Invoke("Activate",delay);
    }
    void Activate(){
        target.SetActive(true);
    }
}
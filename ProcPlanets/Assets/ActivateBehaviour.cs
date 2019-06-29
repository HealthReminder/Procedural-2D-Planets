using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBehaviour : MonoBehaviour
{
    public bool isActiveOnStart = false;
    void Start()
    {
        if(isActiveOnStart)
            gameObject.SetActive(true);
    }
    public void DelayedDeactivate(float delay){
        Invoke("Deactivate",delay);
    }
    void Deactivate(){
        gameObject.SetActive(false);
    }
    public void DelayedActivate(float delay){
        Invoke("Activate",delay);
    }
    void Activate(){
        gameObject.SetActive(true);
    }
}

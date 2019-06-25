using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalNotification : MonoBehaviour
{
    public static GlobalNotification instance;
    private void Awake() {
        instance = this;
    }
    
    public Text notificationText;
    public void Reset() {
        notificationText.text = "";
    }
    public void PostNotification(string message) {
        notificationText.text += message;
    }
}

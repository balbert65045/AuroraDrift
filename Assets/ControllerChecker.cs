using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerChecker : MonoBehaviour
{
    public static ControllerChecker instance;
    public bool usingController = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        bool controllerConnected = false;
        foreach (string name in Input.GetJoystickNames())
        {
            if (name != "")
            {
                controllerConnected = true;
            }
        }
        if (!controllerConnected)
        {
            Debug.Log("Controller Not Plugged in");
            Cursor.visible = true;
            usingController = false;
        }
        else
        {
            Debug.Log("Controller Plugged in");

            Cursor.visible = false;
            usingController = true;
        }
    }
}

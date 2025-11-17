using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButtonDictionary : MonoBehaviour
{
    [SerializeField] Sprite Controller_Movement;
    [SerializeField] Sprite Controller_Pull_Red;
    [SerializeField] Sprite Controller_Pull_Blue;

    [SerializeField] Sprite Keyboard_Movement;
    [SerializeField] Sprite Keyboard_Pull_Red;
    [SerializeField] Sprite Keyboard_Pull_Blue;


    public static TutorialButtonDictionary instance;
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
            return;
        }
    }

    public Sprite GetMovementIcon()
    {
        if (ControllerChecker.instance.usingController)
        {
            return Controller_Movement;
        }
        else
        {
            return Keyboard_Movement;
        }
    }

    public Sprite GetPullRedIcon()
    {
        if (ControllerChecker.instance.usingController)
        {
            return Controller_Pull_Red;
        }
        else
        {
            return Keyboard_Pull_Red;
        }
    }

    public Sprite GetPullBlueIcon()
    {
        if (ControllerChecker.instance.usingController)
        {
            return Controller_Pull_Blue;
        }
        else
        {
            return Keyboard_Pull_Blue;
        }
    }

}

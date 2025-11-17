using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDictionary : MonoBehaviour
{
    public static ButtonDictionary instance;


    [SerializeField] Sprite Controller_DashButton;
    [SerializeField] Sprite Controller_Ability2Button;
    [SerializeField] Sprite Controller_Ability3Button;

    [SerializeField] Sprite Keyboard_DashButton;
    [SerializeField] Sprite Keyboard_Ability2Button;
    [SerializeField] Sprite Keyboard_Ability3Button;
    // Start is called before the first frame update
    void Awake()
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

    public Sprite GetDashIcon()
    {
        if(ControllerChecker.instance.usingController)
        {
            return Controller_DashButton;
        }
        else
        {
            return Keyboard_DashButton;
        }
    }

    public Sprite GetAbility2Icon()
    {
        if (ControllerChecker.instance.usingController)
        {
            return Controller_Ability2Button;
        }
        else
        {
            return Keyboard_Ability2Button;
        }
    }

    public Sprite GetAbility3Icon()
    {
        if (ControllerChecker.instance.usingController)
        {
            return Controller_Ability3Button;
        }
        else
        {
            return Keyboard_Ability3Button;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

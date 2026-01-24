using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonRegion
{
    Dash,
    Ability2,
    Ability3
}

public class ButtonIcon : MonoBehaviour
{
    public ButtonRegion region;
    // Start is called before the first frame update
    void Start()
    {
        SetupAbility();
    }


    public void SetupAbility()
    {
        Sprite mySprite = null;
        switch (region)
        {
            case ButtonRegion.Dash:
                mySprite = ButtonDictionary.instance.GetDashIcon();
                break;
            case ButtonRegion.Ability2:
                mySprite = ButtonDictionary.instance.GetAbility2Icon();
                break;
            case ButtonRegion.Ability3:
                mySprite = ButtonDictionary.instance.GetAbility3Icon();
                break;

        }
        if (GetComponent<Image>() != null)
        {
            GetComponent<Image>().sprite = mySprite;
        }
        else if (GetComponent<SpriteRenderer>() != null)
        {
            GetComponent<SpriteRenderer>().sprite = mySprite;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

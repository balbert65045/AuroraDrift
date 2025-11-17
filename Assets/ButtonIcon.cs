using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIcon : MonoBehaviour
{
    [SerializeField] AbilityRegion region;
    // Start is called before the first frame update
    void Start()
    {
        Sprite mySprite = null;
        switch (region)
        {
            case AbilityRegion.Dash:
                mySprite = ButtonDictionary.instance.GetDashIcon();
                break;
            case AbilityRegion.Ability2:
                mySprite = ButtonDictionary.instance.GetAbility2Icon();
                break;
            case AbilityRegion.Ability3:
                mySprite = ButtonDictionary.instance.GetAbility3Icon();
                break;

        }
        if(GetComponent<Image>() != null)
        {
            GetComponent<Image>().sprite = mySprite;
        }
        else if(GetComponent<SpriteRenderer>() != null)
        {
            GetComponent<SpriteRenderer>().sprite = mySprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

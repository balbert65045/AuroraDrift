using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialButtonType
{
    Movement,
    PullRed,
    PullBlue
}

public class TutorialButton : MonoBehaviour
{
    [SerializeField] TutorialButtonType myType;
    [SerializeField] GameObject Alternate;
    void Start()
    {
        if (Alternate != null)
        {
            if (ControllerChecker.instance.usingController)
            {
                Alternate.SetActive(true);
                GetComponent<Image>().enabled = false;
            }
            return;
        }

        Sprite mySprite = null;
        switch (myType)
        {
            case TutorialButtonType.Movement:
                mySprite = TutorialButtonDictionary.instance.GetMovementIcon();
                break;
            case TutorialButtonType.PullRed:
                mySprite = TutorialButtonDictionary.instance.GetPullRedIcon();
                break;
            case TutorialButtonType.PullBlue:
                mySprite = TutorialButtonDictionary.instance.GetPullBlueIcon();
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
}

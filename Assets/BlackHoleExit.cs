using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleExit : MonoBehaviour
{
    [SerializeField] float InitialGrowTime = 2f;
    [SerializeField] GameObject IndicatorController;

    [SerializeField] GameObject Orb1Spot;
    [SerializeField] GameObject Orb2Spot;

    TimerClass GrowTimer = new TimerClass(false);
    Vector3 initSize;
    // Start is called before the first frame update
    void Start()
    {
        GrowTimer = new TimerClass(true, InitialGrowTime, Time.time);
        initSize = transform.localScale;
        transform.localScale = Vector3.zero;
        FindObjectOfType<TutorialIndicator>().SetNewTarget(this.transform);

    }

    // Update is called once per frame
    void Update()
    {
        if (GrowTimer.IsOn())
        {
            if (GrowTimer.TimerStillGoing(Time.time))
            {
                float percentage = GrowTimer.percentageComplete(Time.time);
                transform.localScale = percentage * initSize;
            }
        }

        if (shrinkTimer.IsOn())
        {
            if (shrinkTimer.TimerStillGoing(Time.time))
            {
                float percentage = shrinkTimer.percentageComplete(Time.time);
                transform.localScale = initSize - (initSize * percentage);
            }
            else
            {
                //Done shrinking
                FindObjectOfType<ScreenTransition>().Grow(transform.position);
            }
        }
    }

    void CheckForEnableExit()
    {
        if (blueEntered && redEntered)
        {
            IndicatorController.SetActive(true);
            FindObjectOfType<PlayerInputController>().EnableNextLevel(this);
        }
        else
        {
            IndicatorController.SetActive(false);
            FindObjectOfType<PlayerInputController>().DissableNextLevel();
        }
    }

    bool blueEntered = false;
    bool redEntered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerMovement>() != null)
        {
            FindObjectOfType<BlueOrbStateController>().EnterBlackHole(Orb1Spot.transform, this.transform);
            FindObjectOfType<PlayerInputController>().DissableDash();
            blueEntered = true;
        }
        if(collision.GetComponent<RedOrbController>() != null)
        {
            FindObjectOfType<RedOrbStateController>().EnterBlackHole(Orb2Spot.transform, this.transform);
            redEntered = true;
        }
        CheckForEnableExit();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Exiting) { return; }
        if (collision.GetComponent<PlayerMovement>() != null)
        {
            FindObjectOfType<BlueOrbStateController>().ExitBlackHole();
            FindObjectOfType<PlayerInputController>().EnableDash();
            blueEntered = false;
        }
        if (collision.GetComponent <RedOrbController>() != null)
        {
            FindObjectOfType<RedOrbStateController>().ExitBlackHole();
            redEntered = false;
        }
        CheckForEnableExit();
    }

    bool Exiting = false;
    TimerClass shrinkTimer = new TimerClass(false);
    public void ShrinkAndExit()
    {
        IndicatorController.SetActive(false);
        Exiting = true;
        float time = 1;
        FindObjectOfType<BlueOrbStateController>().Shrink(time);
        FindObjectOfType<RedOrbStateController>().Shrink(time);
        shrinkTimer = new TimerClass(true, time, Time.time);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1SpinAttackVisual : MonoBehaviour
{
    [SerializeField] Boss1ChargeVisual visual;
    [SerializeField] Boss1ChargeVisual length;

    Color initVisColor;
    Color initLengthColor;
    public void Setup(float time, Vector2 dir)
    {
        visual.OnChargeBegin(time);
        length.OnChargeBegin(time);
    }

    TimerClass doneTimer = new TimerClass(false);
    float fadeTime = .5f;
    public void Finished()
    {
        doneTimer = new TimerClass(true, fadeTime, Time.time);
    }
    // Start is called before the first frame update
    void Start()
    {
        initVisColor = visual.GetComponent<SpriteRenderer>().color;
        initLengthColor = length.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (doneTimer.IsOn())
        {
            if(doneTimer.TimerStillGoing(Time.time))
            {
                float percentage = 1 - doneTimer.percentageComplete(Time.time);
                visual.GetComponent<SpriteRenderer>().color = new Color(initVisColor.r, initVisColor.g, initVisColor.b, percentage * initVisColor.a);
                length.GetComponent<SpriteRenderer>().color = new Color(initLengthColor.r, initLengthColor.g, initLengthColor.b, percentage * initLengthColor.a);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}

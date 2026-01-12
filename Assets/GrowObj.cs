using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowObj : MonoBehaviour
{
    [SerializeField] float growSize = 1f;
    [SerializeField] float growTime = 1f;
    TimerClass growTimer = new TimerClass(false);
    // Start is called before the first frame update
    void Start()
    {
        growTimer = new TimerClass(true, growTime, Time.time);
    }

    // Update is called once per frame
    void Update()
    {
        if (growTimer.IsOn())
        {
            if (growTimer.TimerStillGoing(Time.time))
            {
                float percentage = growTimer.percentageComplete(Time.time);
                transform.localScale = Vector3.one * growSize * percentage;
            }
            else
            {

            }
        }
    }
}

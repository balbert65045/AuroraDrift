using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCanvas : MonoBehaviour
{
    [SerializeField] GameObject Bars;
    [SerializeField] GameObject Name;

    TimerClass GrowTimer = new TimerClass(false);
    public void RevealBoss(float time)
    {
        GrowTimer = new TimerClass(true, time, Time.time);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GrowTimer.IsOn())
        {
            if (GrowTimer.TimerStillGoing(Time.time))
            {
                float percentage = GrowTimer.percentageComplete(Time.time);
                Bars.transform.localScale = new Vector3(percentage, 1, 1);
            }
            else
            {
                Name.SetActive(true);
            }
        }
    }
}

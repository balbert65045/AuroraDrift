using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1ChargeVisual : MonoBehaviour
{
    [SerializeField] float maxStretch = -10f;
    BossBody body;
    [SerializeField] bool IsLength = false;
    // Start is called before the first frame update
    void Start()
    {
        //body = GetComponentInParent<BossBody>();
        //body.OnShowVisual += OnChargeBegin;
    }

    TimerClass visualTimer = new TimerClass(false);
    public void OnChargeBegin(float time)
    {
        if (IsLength)
        {
            transform.localScale = new Vector3(transform.localScale.x, maxStretch, transform.localScale.z);
        }
        visualTimer = new TimerClass(true, time, Time.time);
    }

    // Update is called once per frame
    void Update()
    {
        if (visualTimer.IsOn())
        {
            if (visualTimer.TimerStillGoing(Time.time))
            {
                float percentage = visualTimer.percentageComplete(Time.time);
                if (!IsLength)
                {
                    transform.localScale = new Vector3(transform.localScale.x, percentage * maxStretch, transform.localScale.z);
                }
            }
            else
            {
                if (!IsLength)
                {
                    GetComponentInParent<Boss1SpinAttackVisual>().Finished();
                }
            }
        }
    }
}

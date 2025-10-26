using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStaggerBar : MonoBehaviour
{
    [SerializeField] Image staggerBar;
    [SerializeField] EnemyStagger enemyStagger;

    // Start is called before the first frame update
    void Start()
    {
        enemyStagger.OnStaggerChanged += OnStaggerChanged;
        enemyStagger.OnStagger += Staggered;
    }

    void OnStaggerChanged(HealthStruct staggerStruct)
    {
        float percentage = staggerStruct.Health / staggerStruct.MaxHealth;
        staggerBar.fillAmount = percentage;
    }

    TimerClass timerClass = new TimerClass(false);
    void Staggered(float time)
    {
        timerClass = new TimerClass(true, time, Time.time);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerClass.IsOn())
        {
            if (timerClass.TimerStillGoing(Time.time))
            {
                float percentage = timerClass.percentageComplete(Time.time);
                staggerBar.fillAmount = 1 - percentage;
            }
        }
    }
}

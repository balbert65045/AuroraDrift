using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] float TransitionTime = 1f;
    [SerializeField] float maxSize = 150f;

    TimerClass growTimer = new TimerClass(false);
    public void Grow(Vector3 pos)
    {
        transform.position = pos;
        StartCoroutine("DelayThenGrow");
    }

    IEnumerator DelayThenGrow()
    {
        yield return new WaitForSeconds(.6f);
        growTimer = new TimerClass(true, TransitionTime, Time.time);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        if (growTimer.IsOn())
        {
            if (growTimer.TimerStillGoing(Time.time))
            {
                float percentage = growTimer.percentageComplete(Time.time);
                transform.localScale = Vector3.one * maxSize * percentage;
            }
            else
            {
                LevelManager.instance.LoadNextLevel();
            }
        }
    }
}

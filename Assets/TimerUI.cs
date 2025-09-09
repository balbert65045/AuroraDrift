using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    TMP_Text text;
    bool RecordTime = true;

    public string GetTime()
    {
        return text.text;
    }

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        GameManager gameManager = FindObjectOfType<GameManager>();
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        gameManager.OnCompleteLevel += StopTimer;
        playerHealth.OnDied += StopTimer;
    }

    void StopTimer()
    {
        RecordTime = false;
    }

    string calculateTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 100f) % 100f);
        string formatted = string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
        return formatted;
    }

    // Update is called once per frame
    void Update()
    {
        if(RecordTime)
        {
            text.text = calculateTime(Time.timeSinceLevelLoad);
        }
    }
}

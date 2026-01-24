using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    public void Win()
    {
        FindObjectOfType<TimerUI>().StopTimer();
        FindObjectOfType<PlayerVisual>().gameObject.SetActive(false);
        FindObjectOfType<RedOrbVisual>().gameObject.SetActive(false);
        FindObjectOfType<PlayerInputController>().TakeControl();
        Panel.SetActive(true);
    }
}

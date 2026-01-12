using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] GameObject BallIndicators;

    public void TakeControl()
    {
        BallIndicators.SetActive(false);
    }

    public void ReleaseControl()
    {
        BallIndicators.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

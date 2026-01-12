using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystemCanvas : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    public void TakeControl()
    {
        canvas.enabled = false;
    }

    public void ReleaseControl()
    {
        canvas.enabled = true;
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

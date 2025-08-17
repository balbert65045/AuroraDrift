using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObj : MonoBehaviour
{
    [SerializeField] Transform followObj;
    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (followObj != null)
        {
            transform.position = followObj.position;
        }
    }
}

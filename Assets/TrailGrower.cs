using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailGrower : MonoBehaviour
{
    [SerializeField] float widthMultiplier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TrailRenderer>().widthMultiplier = widthMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float Lifetime = 1f;
    float timeStart;
    // Start is called before the first frame update
    void Start()
    {
        timeStart = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeSinceLevelLoad > timeStart + Lifetime)
        {
            Destroy(this.gameObject);
        }
    }
}

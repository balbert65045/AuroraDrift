using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (FindObjectOfType<PassiveAndAbilitiesManager>() != null)
        {
            Destroy(FindObjectOfType<PassiveAndAbilitiesManager>().gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

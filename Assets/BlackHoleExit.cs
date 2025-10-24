using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleExit : MonoBehaviour
{
    [SerializeField] GameObject Orb1Spot;
    [SerializeField] GameObject Orb2Spot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerMovement>() != null)
        {
            FindObjectOfType<PlayerVisual>().SetTrackObject(Orb1Spot, null);
        }
    }
}

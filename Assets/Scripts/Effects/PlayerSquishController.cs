using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSquishController : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] float squishThreshold = 100f;

    [SerializeField] bool inverse = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.magnitude > squishThreshold)
        {
            float Magnifier = rb.velocity.magnitude / squishThreshold;
            float x = 1 / Magnifier;
            float y = Magnifier;

            float zDegrees = Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.x, rb.velocity.y);

            if(inverse) { 
                zDegrees = Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.y, rb.velocity.x) + 90f;
            }
            
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, zDegrees);
            transform.localScale = new Vector3(x, y, 1);
        }
        else
        {
            transform.localScale = Vector3.one;

        }
    }
}

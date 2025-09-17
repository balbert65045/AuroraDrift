using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalerWithCamera : MonoBehaviour
{
    // Start is called before the first frame update
    Camera cam;
    float initialSize = 21.6f;
    float initLocalSize;
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        initLocalSize = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        float Magnifier = (cam.orthographicSize / initialSize) - 1;
        Magnifier = Magnifier / 2;
        float newSize = initLocalSize + Magnifier*initLocalSize;
        transform.localScale = Vector3.one * newSize;
    }
}

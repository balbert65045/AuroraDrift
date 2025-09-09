using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingLineRenderer : MonoBehaviour
{
    [SerializeField] Transform blue;
    [SerializeField] Transform red;
    PlayerOrbitController orbitController;
    
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        orbitController = FindObjectOfType<PlayerOrbitController>();
        orbitController.OnBeginOrbit += BeginOrbit;
        orbitController.OnEndOrbit += EndOrbit;
    }

    void BeginOrbit()
    {
        lineRenderer.enabled = true;
    }

    void EndOrbit()
    {
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, blue.position);
        lineRenderer.SetPosition(1, red.position);
    }
}

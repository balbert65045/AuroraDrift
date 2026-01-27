using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingLine : MonoBehaviour
{
    SwingController swingController;
    PlayerMovement pm;
    RedOrbController redOrb;

    LineRenderer lineRenderer;
    Vector3 LastPlayerPos = Vector2.zero;

    [SerializeField] bool ForBlue = false;

    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        redOrb = FindObjectOfType<RedOrbController>();
        lineRenderer = GetComponent<LineRenderer>();

        swingController = PassiveAndAbilitiesManager.instance.skillController.swingController;
        swingController.OnSwingBegin += BeginSwing;
        swingController.OnSwingEndRed += EndSwing;
        swingController.OnSwingEndBlue += EndSwing;
    }

    private void OnDestroy()
    {
        swingController.OnSwingBegin -= BeginSwing;
        swingController.OnSwingEndRed -= EndSwing;
        swingController.OnSwingEndBlue -= EndSwing;
    }

    bool showLine = false;
    void BeginSwing(bool blue)
    {
        if(ForBlue != blue) { return; }
        showLine = true;
        lineRenderer.enabled = true;
    }

    void EndSwing()
    {
        showLine = false;
        lineRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        LastPlayerPos = pm.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(redOrb == null) return;
        lineRenderer.SetPosition(0, LastPlayerPos);
        lineRenderer.SetPosition(1, redOrb.transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class mouseLineRenderer : MonoBehaviour
{
    LineRenderer lineRenderer;
    Transform follow;
    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        follow = playerMovement.transform;
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lineRenderer.SetPosition(0, follow.position);
        Vector2 halfway = (mouseWorldPos + (Vector2)follow.position) / 2f;

        lineRenderer.SetPosition(1, halfway);
        lineRenderer.SetPosition(2, mouseWorldPos);
    }
}

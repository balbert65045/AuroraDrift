using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFollow : MonoBehaviour
{
    private void Start()
    {
        PlayerInputController playerInputController = FindObjectOfType<PlayerInputController>();
        if (ControllerChecker.instance.usingController)
        {
            playerInputController.OnMoveInput += RotateInDirection;
        }
        else
        {
            playerInputController.OnMouseDirChanged += PointInDirection;
        }
    }

    public void RotateInDirection(object sender, Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle - 90);
    }

    void PointInDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    private void Update()
    {
        
    }
}

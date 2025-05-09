using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationController : MonoBehaviour
{

    void Update()
    {
        //if(Time.timeScale == 0) { return; }
        //Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mouseWorldPosition.z = 0f; // Ensure we are only rotating in 2D

        //// Calculate the direction from the object's position to the mouse position
        //Vector3 direction = mouseWorldPosition - transform.position;

        //// Compute the angle in radians and then convert to degrees
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //// Apply the rotation around the Z axis to point at the mouse
        //transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    public void ReceiveLookDir(Vector2 dir)
    {
        // Compute the angle in radians and then convert to degrees
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Apply the rotation around the Z axis to point at the mouse
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }
}

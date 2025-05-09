using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedOrbTracker : MonoBehaviour
{
    [SerializeField] RedOrbController redOrb;
    [SerializeField] List<GameObject> ObjectsInRange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            ObjectsInRange.Add(collision.transform.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if(ObjectsInRange.Contains(collision.transform.gameObject))
            {
                ObjectsInRange.Remove(collision.transform.gameObject);
            }
        }
    }

    public GameObject ClosestObject()
    {
        if (ObjectsInRange.Count == 0) { return null; }
        float ClosesDist = 1000;
        GameObject closestObject = null;
        foreach (GameObject _object in ObjectsInRange)
        {
            float dist = (transform.position - _object.transform.position).magnitude;
            if(dist < ClosesDist)
            {
                ClosesDist = dist;
                closestObject = _object;
            }
        }
        return closestObject;   
    }

    private void Update()
    {
        transform.position = redOrb.transform.position;
    }
}

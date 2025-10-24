using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaTutorial : MonoBehaviour
{
    TutorialBlock block;
    private void Start()
    {
        block = GetComponentInParent<TutorialBlock>();
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.GetComponent<PlayerMovement>() != null)
    //    {
    //        block.ShowGroup();
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.GetComponent<PlayerMovement>() != null)
    //    {
    //        block.HideGroup();
    //    }
    //}
}

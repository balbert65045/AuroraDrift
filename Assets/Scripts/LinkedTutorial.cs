using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedTutorial : MonoBehaviour
{
    [SerializeField] GameObject LinkedPiece;

    private void OnEnable()
    {
        LinkedPiece.SetActive(true);
    }

    private void OnDisable()
    {
        LinkedPiece.SetActive(false);
    }
}

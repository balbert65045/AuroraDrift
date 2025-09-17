using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Security.Cryptography;

public class TutorialBlock : MonoBehaviour
{
    TutorialController controller;
    [SerializeField] GameObject ExplosionPrefab;
    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<TutorialController>();
        FindObjectOfType<TutorialIndicator>().SetNewTarget(this.transform);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ShowTutorial();
    }

    void ShowTutorial()
    {
        controller.ShowNextTutorial(transform.position);
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        controller.ShowNextBlock(transform.position);
    }
}

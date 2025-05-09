using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class  PauseCanvas : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    bool paused = false;

    [SerializeField] Camera cam;

    float ogCamSize;
    Vector3 ogSize;
    private void Start()
    {
        ogSize = transform.localScale;
        ogCamSize = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
        float factor = cam.orthographicSize / ogCamSize;
        transform.localScale = new Vector3(ogSize.x * factor, ogSize.y * factor, ogSize.z * factor);
    }

    private void ToggleMenu()
    {
        if (!paused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        paused = true;
        Time.timeScale = 0;
        Panel.SetActive(true);
    }

    public void ResumeGame()
    {
        paused = false;
        Time.timeScale = 1;
        Panel.SetActive(false);
    }
}

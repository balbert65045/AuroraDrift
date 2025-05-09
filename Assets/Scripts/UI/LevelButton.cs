using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public void LoadLevel(string levelName)
    {
        Time.timeScale = 1;
        LevelManager.instance.LoadLevel(levelName);
    }

    public void ReloadLevel()
    {
        Time.timeScale = 1;
        LevelManager.instance.ReloadLevel();
    }

    public void QuitLevel()
    {
        Application.Quit();
    }
}

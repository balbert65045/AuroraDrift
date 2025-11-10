using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;
    public int sceneNumber = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void LoadLevel(string levelName)
    {
        sceneNumber++;
        SceneManager.LoadScene(levelName);
    }

    public void LoadNextLevel()
    {
        sceneNumber++;
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextIndex);
    }

    public void ReloadLevel()
    {
        sceneNumber++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

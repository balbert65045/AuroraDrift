using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassiveAndAbilitiesManager : MonoBehaviour
{
    public static PassiveAndAbilitiesManager instance;

    [SerializeField] Canvas canvas;
    public PlayerSkillController skillController;
    public PlayerPassiveController playerPassiveController;
    public AbilityController abilityController;
    public PlayerHealth playerHealth;
    public UpgradeSystem upgradeSystem;

    public Action OnRecconectPointer;
    ComboSystem comboSystem;

    float currentTime = 0;

    public void SaveTime()
    {
        currentTime = FindObjectOfType<TimerUI>().GetCurrentTime();
    }
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        SceneManager.activeSceneChanged += NewSceneEntered;
        abilityController = gameObject.GetComponent<AbilityController>();
        skillController = GetComponent<PlayerSkillController>();
        playerPassiveController = GetComponent<PlayerPassiveController>();
        playerHealth = GetComponent<PlayerHealth>();
        upgradeSystem = GetComponent<UpgradeSystem>();
        comboSystem = FindObjectOfType<ComboSystem>();

    }

    void ResetValues()
    {
        playerHealth.ResetValues();
        upgradeSystem.ClearUpgrades();
        comboSystem.ClearValues();
    }

    void NewSceneEntered(Scene scene1, Scene scene2)
    {
      if (instance != this) { return; }
      if (LevelManager.instance.sceneNumber == 0) { return; }
      if(scene2.name == "Title")
        {
            //Reset
            ResetValues();
        }
        else
        {
            bool firstLevel = (scene2.name == "Level");
            if (firstLevel)
            {
                ResetValues();
            }
            else
            {
                FindObjectOfType<TimerUI>().InitTimer(currentTime);
            }
            Debug.Log("New Scene entered");
            GetComponentInChildren<SkillIconManager>().SetupDash();
            ReconnectValues();

            playerHealth.Setup(firstLevel);
            if (canvas != null)
            {
                canvas.worldCamera = Camera.main;
            }
        }
    }

    void ReconnectValues()
    {
        skillController.Reconnect();
        abilityController.Reconnect();
        if(OnRecconectPointer != null) { OnRecconectPointer.Invoke(); }
    }

}

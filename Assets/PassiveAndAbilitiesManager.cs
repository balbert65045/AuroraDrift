using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassiveAndAbilitiesManager : MonoBehaviour
{
    public static PassiveAndAbilitiesManager instance;

    [SerializeField] Canvas canvas;
    public PlayerAbilityController abilityController;
    public PlayerPassiveController playerPassiveController;
    public PlayerHealth playerHealth;
    public UpgradeSystem upgradeSystem;

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
        abilityController = GetComponent<PlayerAbilityController>();
        playerPassiveController = GetComponent<PlayerPassiveController>();
        playerHealth = GetComponent<PlayerHealth>();
        upgradeSystem = GetComponent<UpgradeSystem>();
    }

    void ResetValues()
    {
        playerHealth.ResetValues();
        upgradeSystem.ClearUpgrades();
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
            abilityController.Reconnect();
            playerHealth.Setup(firstLevel);
            if (canvas != null)
            {
                canvas.worldCamera = Camera.main;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

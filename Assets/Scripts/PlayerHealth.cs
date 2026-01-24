using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct HealthStruct
{
    public float Health;
    public float MaxHealth;

    public HealthStruct(float health, float maxHealth)
    {
        Health = health;
        MaxHealth = maxHealth;
    }

}

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float MaxHealth = 100;
    public float currentHealth = 100;

    float baseMaxHealth = 0;
    public float GetBaseMaxHealth() { return baseMaxHealth; }

    public Action OnDied;
    public Action<HealthStruct, bool> OnHealthChanged;

    [SerializeField] PlayerChargeController playerChargeController;
    PlayerPassiveController playerPassiveController;
    // Start is called before the first frame update
    void Start()
    {
        baseMaxHealth = MaxHealth;
        currentHealth = MaxHealth;

        playerPassiveController = PassiveAndAbilitiesManager.instance.playerPassiveController;
        if (playerPassiveController != null)
        {
            playerPassiveController.OnHealthIncrease += SetNewMaxHealth;
        }
    }

    public void ResetValues()
    {
        if(baseMaxHealth == 0)
        {
            baseMaxHealth = MaxHealth;
        }
        MaxHealth = baseMaxHealth;
        currentHealth = baseMaxHealth;
    }

    public void Setup(bool firstLevel)
    {
        playerChargeController = FindObjectOfType<PlayerChargeController>();
        //FindObjectOfType<Healthbar>().SetupHealth(new HealthStruct(currentHealth, MaxHealth));

        if (firstLevel)
        {
            MaxHealth = baseMaxHealth;
            currentHealth = baseMaxHealth;
            FindObjectOfType<Healthbar>().SetupHealth(new HealthStruct(baseMaxHealth, baseMaxHealth));
        }
        else
        {
            FindObjectOfType<Healthbar>().SetupHealth(new HealthStruct(currentHealth, MaxHealth));
        }
    }

    private void OnDestroy()
    {
        if (playerPassiveController != null)
        {
            playerPassiveController.OnHealthIncrease -= SetNewMaxHealth;
        }
    }

    public void AddHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        if (OnHealthChanged != null) { OnHealthChanged.Invoke(new HealthStruct(currentHealth, MaxHealth), false); }

    }

    void SetNewMaxHealth(float healthIncrease)
    {
        float diffIncrease = (baseMaxHealth - MaxHealth) + healthIncrease;
        MaxHealth = baseMaxHealth + healthIncrease;
        currentHealth += diffIncrease;

        if (OnHealthChanged != null) { OnHealthChanged.Invoke(new HealthStruct(currentHealth, MaxHealth), false); }
    }

    public void LoseHealth(float amount)
    {
        if (playerChargeController == null)
        {
            playerChargeController = FindObjectOfType<PlayerChargeController>();
        }
        playerChargeController.LoseCharge(100);

        FindObjectOfType<ComboSystem>().ClearCombo();
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        if (OnHealthChanged != null) { OnHealthChanged.Invoke(new HealthStruct(currentHealth, MaxHealth), true); }
        if (currentHealth <= 0)
        {
            Destroy(FindObjectOfType<PlayerMovement>().gameObject);
            Destroy(FindObjectOfType<PlayerVisual>().gameObject);
            Destroy(FindObjectOfType<RedOrbController>().gameObject);
            Destroy(FindObjectOfType<RedOrbVisual>().gameObject);
            if (OnDied != null) { OnDied.Invoke(); }
        }
    }
}

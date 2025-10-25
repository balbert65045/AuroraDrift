using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] GameObject redOrbController;
    [SerializeField] GameObject BlueVisual;
    [SerializeField] GameObject RedVisual;
    [SerializeField] float MaxHealth = 100;
    public float currentHealth;

    float baseMaxHealth;
    public float GetBaseMaxHealth() { return baseMaxHealth; }

    public Action OnDied;
    public Action<HealthStruct, bool> OnHealthChanged;

    PlayerChargeController playerChargeController;
    // Start is called before the first frame update
    void Start()
    {
        baseMaxHealth = MaxHealth;
        currentHealth = MaxHealth;
        playerChargeController = FindObjectOfType<PlayerChargeController>();
        
        PlayerPassiveController playerPassiveController = FindObjectOfType<PlayerPassiveController>();
        if(playerPassiveController != null )
        {
            playerPassiveController.OnHealthIncrease += SetNewMaxHealth;
        }
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
        playerChargeController.LoseCharge(100);
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        if (OnHealthChanged != null) { OnHealthChanged.Invoke(new HealthStruct(currentHealth, MaxHealth), true); }
        if (currentHealth <= 0)
        {
            Destroy(BlueVisual);
            Destroy(RedVisual);
            Destroy(this.gameObject);
            Destroy(redOrbController);
            if (OnDied != null) { OnDied.Invoke(); }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

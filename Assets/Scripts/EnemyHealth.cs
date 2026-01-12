using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DamageType
{
    Blue,
    Orange,
    Purple,
    Red,
    Yellow
}
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float MaxHealth = 30;
    float currentHealth;

    public float GetCurrentHealthPercentage()
    {
        return currentHealth / MaxHealth;
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    [SerializeField] GameObject ExplosionPrefab;
    [SerializeField] GameObject DamageFontPredab;

    [SerializeField] GameObject RedDamageFontPrefab;
    [SerializeField] GameObject PurpleDamageFontPrefab;
    [SerializeField] GameObject OrangeDamageFontPrefab;
    [SerializeField] GameObject YellowDamageFontPrefab;
    [SerializeField] GameObject DamagedPrefab;

    PlayerChargeController playerChargeController;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = MaxHealth;
        playerChargeController = FindObjectOfType<PlayerChargeController>();
    }

    //public DamageType DetermineDamageType(GameObject obj)
    //{
    //    if (obj.GetComponent<PlayerMovement>() != null)
    //    {
    //        if (obj.GetComponent<PlayerMovement>().Orbiting == true)
    //        {
    //            return DamageType.Purple;
    //        }
    //        else
    //        {
    //            return DamageType.Blue;
    //        }
    //    }
    //    else if (obj.GetComponent<RedOrbController>() != null)
    //    {
            
    //    }
    //}

    public EventHandler<HealthStruct> OnTakeDamage;
    public void TakeDamage(DamageType damageType, float amount)
    {
        GameObject fontPrefab = DamageFontPredab;
        switch (damageType)
        {
            case DamageType.Blue:
                playerChargeController.PauseCharge();

                break;
            case DamageType.Purple:
                fontPrefab = PurpleDamageFontPrefab;
                playerChargeController.PauseCharge();

                break;
            case DamageType.Orange:
                fontPrefab = OrangeDamageFontPrefab;
                playerChargeController.PauseCharge();

                break;
            case DamageType.Yellow:
                fontPrefab = YellowDamageFontPrefab;
                playerChargeController.PauseCharge();

                break;
            case DamageType.Red:
                fontPrefab = RedDamageFontPrefab;
                break;
        }

        currentHealth -= amount;
        float increase = amount >= 7 ? 1.5f : 1;
        if (currentHealth <= 0)
        {
            Explode();
        }
        else
        {
            SpawnDamaged();
        }

        GameObject fontObj = Instantiate(fontPrefab, transform.position, Quaternion.identity);
        fontObj.GetComponent<DamageFont>().DisplayPain(Mathf.FloorToInt(amount), Color.red, increase);
        if (OnTakeDamage != null) { OnTakeDamage.Invoke(this, new HealthStruct(currentHealth, MaxHealth)); }
    }



    public EventHandler<GameObject> OnDeath;
    void Explode()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);

        if (OnDeath != null) { OnDeath.Invoke(this, transform.parent.gameObject); }
        Destroy(this.transform.parent.gameObject);
    }


    void SpawnDamaged()
    {
        Instantiate(DamagedPrefab, transform.position, Quaternion.identity);
    }
}

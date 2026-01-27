using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityContainer : MonoBehaviour
{
    public AbilityIcon CurrentAbility;
    [SerializeField] Image CooldownImage;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ClearAbility()
    {
        if(CurrentAbility != null)
        {
            Destroy(CurrentAbility.gameObject);
            CurrentAbility = null;
        }
    }

    public void SetCombinationAbility(GameObject iconPrefab)
    {
        GameObject iconObj = Instantiate(iconPrefab, this.transform);
        CurrentAbility = iconObj.GetComponent<AbilityIcon>();
        SetupCombinationAbility(CurrentAbility);
    }

    void SetupCombinationAbility(AbilityIcon icon)
    {
        FindObjectOfType<AbilityController>().OnStartCombineCooldown += SetAbilityOnCooldown;
    }


    public void SetAbility(GameObject iconPrefab)
    {
        GameObject iconObj = Instantiate(iconPrefab, this.transform);
        CurrentAbility = iconObj.GetComponent<AbilityIcon>();
        SetupAbility(CurrentAbility);
    }

    void SetupAbility(AbilityIcon icon)
    {
        switch (icon.orbType)
        {
            case OrbType.Blue:
                FindObjectOfType<AbilityController>().OnStartBlueCooldown += SetAbilityOnCooldown;
                break;
            case OrbType.Red:
                FindObjectOfType<AbilityController>().OnStartRedCooldown += SetAbilityOnCooldown;
                break;
        }
    }

    TimerClass cooldownTimer = new TimerClass(false);
    void SetAbilityOnCooldown(float time)
    {
        cooldownTimer = new TimerClass(true, time, Time.time);
    }

    void Update()
    {
        if (cooldownTimer.IsOn())
        {
            if (cooldownTimer.TimerStillGoing(Time.time))
            {
                float percentage = cooldownTimer.percentageComplete(Time.time);
                CooldownImage.fillAmount = 1 - percentage;
            }
            else
            {
                CooldownImage.fillAmount = 0;
            }
        }
    }

}

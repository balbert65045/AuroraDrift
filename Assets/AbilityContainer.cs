using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum AbilityRegion
{
    Dash,
    Ability2,
    Ability3
}

public class AbilityContainer : MonoBehaviour
{
    [SerializeField] AbilityRegion abilityRegion;
    [SerializeField] Image CooldownImage;

    [SerializeField] AbilityIcon CurrentAbility;
    [SerializeField] Image currentIcon;
    [SerializeField] Image altIcon;

    // Start is called before the first frame update
    void Start()
    {
        switch (abilityRegion)
        {
            case AbilityRegion.Dash:
                FindObjectOfType<PlayerAbilityController>().OnDashAbility += SetAbilityOnCooldown;
                FindObjectOfType<PlayerOrbitController>().OnBeginOrbit += SwitchToAlt;
                FindObjectOfType<PlayerOrbitController>().OnEndOrbit += SwitchOffAlt;
                break;
        }
    }

    void SwitchToAlt()
    {
        if(altIcon == null) { return; }
        if(CurrentAbility != null) { return; }
        altIcon.gameObject.SetActive(true);
        currentIcon.gameObject.SetActive(false);
    }

    void SwitchOffAlt()
    {
        if (altIcon == null) { return; }
        if (CurrentAbility != null) { return; }
        altIcon.gameObject.SetActive(false);
        currentIcon.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        ClearAbility();
    }

    public void ClearAbility()
    {
        switch (abilityRegion)
        {
            case AbilityRegion.Dash:
                //if()
                //FindObjectOfType<PlayerAbilityController>().OnDashAbility -= SetAbilityOnCooldown;
                FindObjectOfType<PlayerOrbitController>().OnBeginOrbit -= SwitchToAlt;
                FindObjectOfType<PlayerOrbitController>().OnEndOrbit -= SwitchOffAlt;
                break;
        }

        if (CurrentAbility != null)
        {
            switch (CurrentAbility.GetAbilityType())
            {
                case AbilityType.OrbLaunch:
                    FindObjectOfType<OrbLaunchController>().OnEnableCharge -= EnableIcon;
                    FindObjectOfType<OrbLaunchController>().OnDisableCharge -= DisableIcon;
                    break;
                case AbilityType.Swap:
                    FindObjectOfType<SwapController>().OnStartCooldown -= SetAbilityOnCooldown;
                    break;
            }
            Destroy(CurrentAbility.gameObject);
            CurrentAbility = null;
        }
    }

    public void SetAbility(GameObject iconPrefab)
    {
        if(CurrentAbility != null)
        {
            CurrentAbility.IncreaseQuantity();
            return;
        }

        GameObject iconObj = Instantiate(iconPrefab, this.transform);
        CurrentAbility = iconObj.GetComponent<AbilityIcon>();
        if (currentIcon != null)
        {
            iconObj.SetActive(false);
        }

        SetUpAbility(CurrentAbility);
    }

    void SetUpAbility(AbilityIcon icon)
    {
        switch (icon.GetAbilityType())
        {
            case AbilityType.OrbLaunch:
                FindObjectOfType<OrbLaunchController>().OnEnableCharge += EnableIcon;
                FindObjectOfType<OrbLaunchController>().OnDisableCharge += DisableIcon;
                break;
            case AbilityType.Swap:
                FindObjectOfType<SwapController>().OnStartCooldown += SetAbilityOnCooldown;
                break;
        }
    }

    public void Setup()
    {
        Debug.Log("Setting up");
        FindObjectOfType<PlayerOrbitController>().OnBeginOrbit += SwitchToAlt;
        FindObjectOfType<PlayerOrbitController>().OnEndOrbit += SwitchOffAlt;
    }

    void EnableIcon()
    {
        if (CurrentAbility != null)
        {
            CurrentAbility.gameObject.SetActive(true);
            currentIcon.gameObject.SetActive(false);
        }
    }

    void DisableIcon()
    {
        if(CurrentAbility != null)
        {
            CurrentAbility.gameObject.SetActive(false);
            currentIcon.gameObject.SetActive(true);
        }
    }

    TimerClass cooldownTimer = new TimerClass(false);
    void SetAbilityOnCooldown(float time)
    {
        cooldownTimer = new TimerClass(true, time, Time.time);
    }
    // Update is called once per frame
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

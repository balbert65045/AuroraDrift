using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveIconManager : MonoBehaviour
{
    [SerializeField] PassiveIcon[] passiveIconPrefabs;
    
    List<PassiveIcon> CurrentIcons = new List<PassiveIcon>();
    UpgradeSystem upgradeSystem;
    // Start is called before the first frame update
    void Start()
    {
        upgradeSystem = FindObjectOfType<UpgradeSystem>();
        upgradeSystem.OnSelectPassive += PassiveSelected;
    }

    void PassiveSelected(Upgrade passiveUpgrade)
    {
        PassiveIcon currentIcon = IconAvailable(passiveUpgrade.passiveType, passiveUpgrade.orbType);
        if (currentIcon != null)
        {
            currentIcon.IncreaseQuantity();
        }
        else
        {
            foreach (PassiveIcon passiveIcon in passiveIconPrefabs)
            {
                if (passiveIcon.GetPassiveType() == passiveUpgrade.passiveType && ((passiveUpgrade.orbType == OrbType.None) || (passiveUpgrade.orbType == passiveIcon.GetOrbType())))
                {
                    GameObject PassiveIconObj = Instantiate(passiveIcon.gameObject, this.transform);
                    CurrentIcons.Add(PassiveIconObj.GetComponent<PassiveIcon>());
                    return;
                }
            }
        }
    }


    PassiveIcon IconAvailable(PassiveType passiveType, OrbType orbType)
    {
        foreach(PassiveIcon icon in CurrentIcons)
        {
            if(icon.GetPassiveType() == passiveType && icon.GetOrbType() == orbType)
            {
                return icon;
            }
        }
        return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

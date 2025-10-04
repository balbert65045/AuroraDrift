using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveIconManager : MonoBehaviour
{
    [SerializeField] PassiveIcon[] passiveIconPrefabs;
    
    UpgradeSystem upgradeSystem;
    // Start is called before the first frame update
    void Start()
    {
        upgradeSystem = FindObjectOfType<UpgradeSystem>();
        upgradeSystem.OnSelectPassive += PassiveSelected;
    }

    void PassiveSelected(PassiveType selectedType, OrbType orbType)
    {
        foreach(PassiveIcon passiveIcon in passiveIconPrefabs)
        {
            if (passiveIcon.GetPassiveType() == selectedType && ((orbType == OrbType.None) || (orbType == passiveIcon.GetOrbType())))
            {
                Instantiate(passiveIcon.gameObject, this.transform);
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

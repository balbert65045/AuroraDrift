using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PassiveAbilityType
{
    Missel
}

[System.Serializable]
public class PassiveAbilityDictionary
{
    public PassiveAbilityType type;
    public float BaseValue;
    public float ValueIncrease;
    public float Cooldown;
    public float CooldownDecrease;
    public OrbType orbType;
}

[CreateAssetMenu(fileName = "PassiveAbilityDictionary", menuName = "ScriptableObjects/PassiveAbilityDictionary", order = 1)]
public class PassiveAbilityList : ScriptableObject
{
    public List<PassiveAbilityDictionary> passiveAbilityDictionaries;

    public PassiveAbilityDictionary GetPassiveAbilityDictionary(PassiveAbilityType type)
    {
        for (int i = 0; i < passiveAbilityDictionaries.Count; i++)
        {
            if (type == passiveAbilityDictionaries[i].type) { return passiveAbilityDictionaries[i]; }
        }
        return null;
    }

    public float GetBaseValue(PassiveAbilityType type)
    {
        for (int i = 0; i < passiveAbilityDictionaries.Count; i++)
        {
            if (type == passiveAbilityDictionaries[i].type) { return passiveAbilityDictionaries[i].BaseValue; }
        }
        return 0;
    }

    public float GetValueIncrease(PassiveAbilityType type)
    {
        for (int i = 0; i < passiveAbilityDictionaries.Count; i++)
        {
            if (type == passiveAbilityDictionaries[i].type) { return passiveAbilityDictionaries[i].ValueIncrease; }
        }
        return 0;
    }

    public float GetCooldown(PassiveAbilityType type)
    {
        for (int i = 0; i < passiveAbilityDictionaries.Count; i++)
        {
            if (type == passiveAbilityDictionaries[i].type) { return passiveAbilityDictionaries[i].Cooldown; }
        }
        return 0;
    }

}

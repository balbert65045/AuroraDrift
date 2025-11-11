using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityDictionary
{
    public AbilityType type;
    public float BaseValue;
    public float ValueIncrease;
    public float Cooldown;
    public float CooldownDecrease;
}

[CreateAssetMenu(fileName = "AbilityDictionary", menuName = "ScriptableObjects/AbilityDictionary", order = 1)]

public class AbilityList : ScriptableObject
{
    public List<AbilityDictionary> abilityDictionaries;

    public AbilityDictionary GetAbilityDictionary(AbilityType type)
    {
        for (int i = 0; i < abilityDictionaries.Count; i++)
        {
            if (type == abilityDictionaries[i].type) { return abilityDictionaries[i]; }
        }
        return null;
    }

    public float GetBaseValue(AbilityType type)
    {
        for (int i = 0; i < abilityDictionaries.Count; i++)
        {
            if (type == abilityDictionaries[i].type) { return abilityDictionaries[i].BaseValue; }
        }
        return 0;
    }

    public float GetValueIncrease(AbilityType type)
    {
        for (int i = 0; i < abilityDictionaries.Count; i++)
        {
            if (type == abilityDictionaries[i].type) { return abilityDictionaries[i].ValueIncrease; }
        }
        return 0;
    }

    public float GetCooldown(AbilityType type)
    {
        for (int i = 0; i < abilityDictionaries.Count; i++)
        {
            if (type == abilityDictionaries[i].type) { return abilityDictionaries[i].Cooldown; }
        }
        return 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityDictionary
{
    public AbilityType type;
    public float Value;
    public float Cooldown;
}

[CreateAssetMenu(fileName = "AbilityDictionary", menuName = "ScriptableObjects/AbilityDictionary", order = 1)]

public class AbilityList : ScriptableObject
{
    public List<AbilityDictionary> abilityDictionaries;

    public float GetValue(AbilityType type)
    {
        for (int i = 0; i < abilityDictionaries.Count; i++)
        {
            if (type == abilityDictionaries[i].type) { return abilityDictionaries[i].Value; }
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

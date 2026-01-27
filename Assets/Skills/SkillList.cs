using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillDictionary
{
    public SkillType type;
    public float BaseValue;
    public float ValueIncrease;
    public float Cooldown;
    public float CooldownDecrease;
}

[CreateAssetMenu(fileName = "SkillDictionary", menuName = "ScriptableObjects/SkillDictionary", order = 1)]

public class SkillList : ScriptableObject
{
    public List<SkillDictionary> skillDictionaries;

    public SkillDictionary GetAbilityDictionary(SkillType type)
    {
        for (int i = 0; i < skillDictionaries.Count; i++)
        {
            if (type == skillDictionaries[i].type) { return skillDictionaries[i]; }
        }
        return null;
    }

    public float GetBaseValue(SkillType type)
    {
        for (int i = 0; i < skillDictionaries.Count; i++)
        {
            if (type == skillDictionaries[i].type) { return skillDictionaries[i].BaseValue; }
        }
        return 0;
    }

    public float GetValueIncrease(SkillType type)
    {
        for (int i = 0; i < skillDictionaries.Count; i++)
        {
            if (type == skillDictionaries[i].type) { return skillDictionaries[i].ValueIncrease; }
        }
        return 0;
    }

    public float GetCooldown(SkillType type)
    {
        for (int i = 0; i < skillDictionaries.Count; i++)
        {
            if (type == skillDictionaries[i].type) { return skillDictionaries[i].Cooldown; }
        }
        return 0;
    }
}

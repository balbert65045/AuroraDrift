using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PassiveDictionary
{
    public PassiveType type;
    public float Value;
}


[CreateAssetMenu(fileName = "PassiveDictionary", menuName = "ScriptableObjects/PassiveDictionary", order = 1)]

public class PassiveList : ScriptableObject
{
    public List<PassiveDictionary> passiveDictionaries;

    public float GetValue(PassiveType type)
    {
        for (int i = 0; i < passiveDictionaries.Count; i++)
        {
            if (type == passiveDictionaries[i].type) { return passiveDictionaries[i].Value; }
        }
        return 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityCardList", menuName = "ScriptableObjects/AbilityCardList", order = 1)]

public class AbilityCardList : ScriptableObject
{
    public GameObject[] CardPrefabs;

}

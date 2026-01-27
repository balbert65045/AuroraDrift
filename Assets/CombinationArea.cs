using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombinationArea : MonoBehaviour
{

    Animator animator;
    [SerializeField] GameObject Ability1;
    [SerializeField] GameObject Ability2;

    [SerializeField] AbilityIconList IconList;
    [SerializeField] AbilityCardList combinationList;

    [SerializeField] GameObject CardArea;
    [SerializeField] GameObject Container;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        FindAnyObjectByType<UpgradeSystem>().OnCombineAbility += CombineAbilities;
        FindAnyObjectByType<UpgradeSystem>().OnSelectUpgrade += ClearArea;

        //Time.timeScale = 0;

        //Place Ability Icon1 and Ability Icon2
        //CombineAbilities(AbilityType.Missel, AbilityType.Missel, new Upgrade(UpgradeType.Ability, PassiveType.Speed, SkillType.RedSwing, AbilityType.MachineGun, OrbType.None, 0));

    }

    private void OnDestroy()
    {
        FindAnyObjectByType<UpgradeSystem>().OnCombineAbility -= CombineAbilities;
        FindAnyObjectByType<UpgradeSystem>().OnSelectUpgrade -= ClearArea;
    }

    public void HighlightCard()
    {
        if (ControllerChecker.instance.usingController)
        {
            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            eventSystem.SetSelectedGameObject(CardArea.transform.GetChild(0).gameObject);
        }
    }

    public void CombineAbilities(AbilityType abilityBlue, AbilityType abilityRed, Upgrade combinationUpgrade)
    {
        SetupIcons(abilityBlue, abilityRed);

        SetupCombinationCard(combinationUpgrade);

        animator.SetBool("Combine", true);
    }

    void SetupCombinationCard(Upgrade upgrade)
    {
        GameObject Card = null;
        foreach(GameObject gameObject in combinationList.CardPrefabs)
        {
            AbilityCard abilityCard = gameObject.GetComponent<AbilityCard>();
            if(abilityCard.GetAbilityType() == upgrade.abilityType)
            {
                Card = Instantiate(gameObject, CardArea.transform);
            }
        }
        Card.GetComponent<CardUpgrade>().SetupUpgrade(upgrade);

    }


    void SetupIcons(AbilityType abilityBlue, AbilityType abilityRed)
    {
        foreach(GameObject gameObject in IconList.IconPrefabs)
        {
            AbilityIcon icon = gameObject.GetComponent<AbilityIcon>();
            if(icon.abilityType == abilityBlue && icon.orbType == OrbType.Blue)
            {
                Instantiate(gameObject, Ability1.transform);
                break;
            }
        }

        foreach (GameObject gameObject in IconList.IconPrefabs)
        {
            AbilityIcon icon = gameObject.GetComponent<AbilityIcon>();
            if (icon.abilityType == abilityRed && icon.orbType == OrbType.Red)
            {
                Instantiate(gameObject, Ability2.transform);
                break;
            }
        }
    }

    void ClearArea()
    {
        animator.SetBool("Combine", false);
        if (Ability1.transform.childCount > 0)
        {
            Destroy(Ability1.transform.GetChild(0).gameObject);
        }
        if (Ability2.transform.childCount > 0)
        {
            Destroy(Ability2.transform.GetChild(0).gameObject);
        }
        if(CardArea.transform.childCount > 0)
        {
            Destroy(CardArea.transform.GetChild(0).gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}

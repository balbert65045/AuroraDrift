using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UpgradeCanvas : MonoBehaviour
{
    [SerializeField] Animator UpgradeAnimator;
    UpgradeSystem upgradeSystem;
    // Start is called before the first frame update
    void Start()
    {
        upgradeSystem = GetComponentInParent<UpgradeSystem>();
        upgradeSystem.OnShowUpgrades += ShowCanvas;
        upgradeSystem.OnSelectPassive += PassiveSelected;
        upgradeSystem.OnSelectAbility += AbilitySelected;
    }

    void PassiveSelected(PassiveType _p, OrbType _o)
    {
        HideCanvas();
    }

    void AbilitySelected(AbilityType _a)
    {
        HideCanvas();
    }

    void ShowCanvas(object sender, List<Upgrade> _upgrades)
    {
        UpgradeAnimator.SetBool("Show", true);
    }

    void HideCanvas()
    {
        UpgradeAnimator.SetBool("Show", false);
    }

    public void EnableSelect()
    {
        GetComponentInChildren<AvailableUpgradeViewer>().SelectCard();
    }
}

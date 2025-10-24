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
        upgradeSystem.OnSelectUpgrade += HideCanvas;
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

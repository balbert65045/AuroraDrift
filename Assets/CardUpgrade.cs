using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUpgrade : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] GameObject SelectVisual;
    public void OnDeselect(BaseEventData eventData)
    {
        SelectVisual.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        SelectVisual.SetActive(true);
    }


    public void Pressed()
    {
        upgradeSystem.SelectUpgrade(this);
    }

    UpgradeSystem upgradeSystem;
    // Start is called before the first frame update
    void Start()
    {
        upgradeSystem = FindObjectOfType<UpgradeSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

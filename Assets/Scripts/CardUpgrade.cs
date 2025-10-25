using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUpgrade : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject SelectVisual;
    Upgrade upgrade;

    public virtual void SetupUpgrade(Upgrade upgrade)
    {
        this.upgrade = upgrade;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        SelectVisual.SetActive(false);
        animator.SetBool("Grow", false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        SelectVisual.SetActive(true);
        animator.SetBool("Grow", true);
    }


    public void Pressed()
    {
        upgradeSystem.SelectUpgrade(this.upgrade);
    }

    UpgradeSystem upgradeSystem;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        upgradeSystem = FindObjectOfType<UpgradeSystem>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectVisual.SetActive(true);
        animator.SetBool("Grow", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SelectVisual.SetActive(false);
        animator.SetBool("Grow", false);
    }
}

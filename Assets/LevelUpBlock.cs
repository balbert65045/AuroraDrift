using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpBlock : MonoBehaviour
{
    [SerializeField] GameObject ExplosionPrefab;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider2D;
    UpgradeSystem upgradeSystem;

    // Start is called before the first frame update
    void Start()
    {
        upgradeSystem = FindObjectOfType<UpgradeSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        FindObjectOfType<TutorialIndicator>().SetNewTarget(this.transform);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ShowUpgrade();
    }

    void ShowUpgrade()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
        StartCoroutine("WaitThenShow");
    }

    IEnumerator WaitThenShow()
    {
        yield return new WaitForSeconds(.2f);
        upgradeSystem.ShowPossibleUpgrades();
        yield return new WaitForSeconds(.2f);
        Destroy(this.gameObject);


    }
}

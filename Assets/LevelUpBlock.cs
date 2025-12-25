using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpBlock : MonoBehaviour
{
    [SerializeField] float InitialGrowTime = 3f;
    [SerializeField] float RotateSpeed = 100f;
    [SerializeField] GameObject ExplosionPrefab;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider2D;
    UpgradeSystem upgradeSystem;

    TimerClass GrowTimer = new TimerClass(false);
    Vector3 initialScale;
    // Start is called before the first frame update
    void Start()
    {
        upgradeSystem = FindObjectOfType<UpgradeSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        FindObjectOfType<TutorialIndicator>().SetNewTarget(this.transform);
        GrowTimer = new TimerClass(true, InitialGrowTime, Time.time);
        initialScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (GrowTimer.IsOn())
        {
            if (GrowTimer.TimerStillGoing(Time.time))
            {
                float percentage = GrowTimer.percentageComplete(Time.time);
                transform.localScale = percentage * initialScale;
            }
        }
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward, Time.fixedDeltaTime * RotateSpeed);
    }

    bool showingUpgrade = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!showingUpgrade)
        {
            showingUpgrade = true;
            ShowUpgrade();
        }
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
        FindObjectOfType<EnemySpawner>().SpawnNextWave();
        Destroy(this.gameObject);


    }
}

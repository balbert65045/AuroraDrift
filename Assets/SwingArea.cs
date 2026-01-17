using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwingArea : MonoBehaviour
{

    CircleCollider2D circleCollider;
    PlayerMovement pm;
    RedOrbController redOrb;

    public bool IsBlue = false;

    [SerializeField] SpriteRenderer spriteRenderer;

    float radius;
    private void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        redOrb = FindObjectOfType<RedOrbController>();
        radius = GetComponent<CircleCollider2D>().radius;
    }

    public bool PlayerInSwingRange()
    {
        return (pm.transform.position - redOrb.transform.position).magnitude <= radius;
    }

    public void ShowRange()
    {
        spriteRenderer.enabled = true;

//        StartCoroutine("ShowRangeForABit");
    }

    IEnumerator ShowRangeForABit()
    {
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(.2f);
        HideRange();
    }

    public void HideRange()
    {
        spriteRenderer.enabled = false;
    }
}

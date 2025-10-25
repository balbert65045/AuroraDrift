using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAttackPreview : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] LineRenderer lineRenderer2;
    [SerializeField] float Distance = 200;
    [SerializeField] float StartingAngle = 30f;


    float currentAngle;
    Ship ship;

    bool showAttack = false;
    float timeStartedToShowAttack;
    float timeForPreview;
    // Start is called before the first frame update
    void Start()
    {
        ship = GetComponentInParent<Ship>();
        ship.OnAboutToAttack += AboutToShoot;
        currentAngle = StartingAngle;
    }

    void AboutToShoot(object sender, float time)
    {
        timeForPreview = time;
        timeStartedToShowAttack = Time.time;
        showAttack = true;
        lineRenderer.enabled = true;
        lineRenderer2.enabled = true;
    }

    void CreateAngle()
    {

        float currentZ = transform.rotation.eulerAngles.z;
        float angle = currentZ + currentAngle;
        float rad = angle * Mathf.Deg2Rad;

        // Create the direction vector
        Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, (Vector2)transform.position + direction * Distance);

        angle = currentZ - currentAngle;
        rad = angle * Mathf.Deg2Rad;

        // Create the direction vector
        Vector2 direction2 = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        lineRenderer2.SetPosition(0, transform.position);
        lineRenderer2.SetPosition(1, (Vector2)transform.position + direction2 * Distance);

        float percentage = GetPercentageComplete();
        float angleChangePercentage = Mathf.Clamp01(percentage * 1.1f);
        currentAngle = StartingAngle - StartingAngle * angleChangePercentage;
        if(percentage == 1)
        {
            showAttack = false;
            lineRenderer.enabled = false;
            lineRenderer2.enabled = false;
        }
    }

    float GetPercentageComplete()
    {
        return Mathf.Clamp01((Time.time - timeStartedToShowAttack) / (timeForPreview));
    }

    // Update is called once per frame
    void Update()
    {
        if (showAttack)
        {
            CreateAngle();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitDashVisual : MonoBehaviour
{
    [SerializeField] Transform t2;
    [SerializeField] float minSize = .1f;
    [SerializeField] float maxSize = 1f;

    TimerClass currentTimer;
    bool charging = false;

    float diff;

    SpriteRenderer spriteRenderer;
    SpriteRenderer t2SpriteRenderer;

    PlayerAbilityController playerAbilityController;
    PlayerInputController playerInputController;
    // Start is called before the first frame update
    void Start()
    {
        playerInputController = FindObjectOfType<PlayerInputController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        t2SpriteRenderer = t2.GetComponent<SpriteRenderer>();
        playerAbilityController = FindObjectOfType<PlayerAbilityController>();
        playerAbilityController.OnBeginCharge += BeginCharge;
        playerAbilityController.OnReleaseCharge += ReleaseCharge;
        diff = maxSize - minSize;
    }

    void ReleaseCharge()
    {
        t2SpriteRenderer.enabled = false;
        spriteRenderer.enabled = false;
        charging = false;
    }

    void BeginCharge(object sender, TimerClass timer)
    {
        t2SpriteRenderer.enabled = true;
        spriteRenderer.enabled = true;
        currentTimer = timer;
        charging = true;
        transform.localScale = new Vector3(transform.localScale.x, minSize, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (charging)
        {
            float percentage = currentTimer.percentageComplete(Time.timeSinceLevelLoad);
            float evaluation = playerAbilityController.ChargeAnimationCurve.Evaluate(percentage);
            float colorEvaluation = evaluation < .95f ? evaluation - .1f : evaluation;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, colorEvaluation);
            float ySize = minSize + evaluation * diff;
            transform.localScale = new Vector3(transform.localScale.x, ySize, transform.localScale.z);

            t2.transform.localScale = new Vector3(0f + 1f * colorEvaluation, 1, 1);
        }
    }
}

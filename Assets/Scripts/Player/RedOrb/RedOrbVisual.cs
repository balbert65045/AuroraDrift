using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedOrbVisual : MonoBehaviour
{
    public GameObject TrackObject;
    [SerializeField] float speed = 10f;

    //private void Start()
    //{
    //    Material trailMat = new Material(Shader.Find("Unlit/Transparent"));
    //    trailMat.mainTexture = GenerateGradientTexture(Color.white, new Color(1, 1, 1, 0));
    //    GetComponent<TrailRenderer>().material = trailMat;
    //}

    //Texture2D GenerateGradientTexture(Color startColor, Color endColor, int width = 256, int height = 1)
    //{
    //    Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
    //    for (int x = 0; x < width; x++)
    //    {
    //        float t = (float)x / (width - 1);
    //        Color color = Color.Lerp(startColor, endColor, t);
    //        for (int y = 0; y < height; y++)
    //        {
    //            texture.SetPixel(x, y, color);
    //        }
    //    }
    //    texture.Apply();
    //    return texture;
    //}
    OrbLaunchController launchController;
    PlayerAbilityController pbc;
    Color OGColor;
    SpriteRenderer spriteRenderer;
    TrailRenderer trailRenderer;
    private void Start()
    {
        OGSize = transform.localScale;
        transform.localScale = Vector3.zero;
        growShrinkTime = .3f;
        Grow();

        RedOrbStateController stateController = FindObjectOfType<RedOrbStateController>();
        stateController.OnEnterBlackHole += EnterBlackHole;
        stateController.OnExitBlackHole += ExitBlackHole;
        stateController.OnShrink += Shrink;


        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        OGColor = spriteRenderer.color;
        pbc = PassiveAndAbilitiesManager.instance.abilityController;
        launchController = pbc.launchController;
        launchController.OnBeginCharge += ChargeHappening;
        pbc.swapController.OnSwapBegin += Shrink;
        pbc.swapController.OnSwapEnd += Grow;

        RedOrbController redOrb = FindObjectOfType<RedOrbController>();
        redOrb.OnRemoveChargeThrown += ChargeRemoved;
    }

    private void OnDestroy()
    {
        launchController.OnBeginCharge -= ChargeHappening;
        pbc.swapController.OnSwapBegin -= Shrink;
        pbc.swapController.OnSwapEnd -= Grow;
    }

    TimerClass shrinkTimer;
    TimerClass growTimer;
    bool shrinkin = false;
    bool growing = false;
    Vector3 OGSize;
    float growShrinkTime;
    void Shrink(float swapTime)
    {
        growShrinkTime = swapTime;
        shrinkTimer = new TimerClass(true, growShrinkTime, Time.time);
        //OGSize = transform.localScale;
        shrinkin = true;
        growing = false;
    }

    void Grow()
    {
        GetComponent<PlayerSquishController>().PauseSquish();
        growTimer = new TimerClass(true, growShrinkTime/2, Time.time);
        growing = true;
        shrinkin = false;
    }

    private void Update()
    {
        if (shrinkin)
        {
            if (shrinkTimer.TimerStillGoing(Time.time))
            {
                float percentage = shrinkTimer.percentageComplete(Time.time);
                transform.localScale = OGSize - (OGSize * percentage);
            }
            else
            {
                transform.localScale = Vector3.zero;
                shrinkin = false;
            }
        }
        else if (growing && blackHolePos == null)
        {
            if (growTimer.TimerStillGoing(Time.time))
            {
                float percentage = growTimer.percentageComplete(Time.time);
                transform.localScale = (OGSize * percentage);
            }
            else
            {
                GetComponent<PlayerSquishController>().UnPauseSquish();
                growing = false;
            }
        }
    }

    bool charging = false;
    TimerClass currentTimer;
    void ChargeHappening(object sender, TimerClass timer)
    {
        charging = true;
        currentTimer = timer;
        //spriteRenderer.color = Color.white;
        //trailRenderer.startColor = Color.white;
        //trailRenderer.endColor = Color.white;
    }

    void ChargeRemoved()
    {
        charging = false;
        spriteRenderer.color = OGColor;
        trailRenderer.startColor = OGColor;
        trailRenderer.endColor = OGColor;
    }

    public void SetTrackObject(GameObject trackObject, Rigidbody2D rb)
    {
        TrackObject = trackObject;
        GetComponent<PlayerSquishController>().rb = rb;
    }

    Transform blackHolePos;
    TimerClass EnterBlackHoleTimer = new TimerClass(false);

    void EnterBlackHole(Transform t, Transform BlackHolePos)
    {
        GetComponent<PlayerSquishController>().PauseSquish();
        blackHolePos = t;
        EnterBlackHoleTimer = new TimerClass(true, 1, Time.time);

    }

    TimerClass ExitBlackHoleTimer = new TimerClass(false);

    void ExitBlackHole()
    {
        GetComponent<PlayerSquishController>().UnPauseSquish();
        blackHolePos = null;
        ExitBlackHoleTimer = new TimerClass(true, 1, Time.time);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (blackHolePos != null)
        {
            if (EnterBlackHoleTimer.IsOn())
            {
                if (EnterBlackHoleTimer.TimerStillGoing(Time.time))
                {
                    float percentage = EnterBlackHoleTimer.percentageComplete(Time.time);
                    Vector2 diff = blackHolePos.transform.position - transform.position;
                    transform.position = (Vector2)transform.position + diff * percentage;
                }
                return;
            }
            transform.position = Vector2.Lerp(blackHolePos.position, transform.position, Time.deltaTime * speed);

            //transform.position = Vector2.Lerp(transform.position, blackHolePos.position, Time.deltaTime * 3);
            return;
        }
        if (ExitBlackHoleTimer.IsOn())
        {
            if (ExitBlackHoleTimer.TimerStillGoing(Time.time))
            {
                float percentage = ExitBlackHoleTimer.percentageComplete(Time.time);
                Vector2 diff = TrackObject.transform.position - transform.position;
                transform.position = (Vector2)transform.position + diff * percentage;
            }
            return;
        }
        transform.position = Vector2.Lerp(TrackObject.transform.position, transform.position, Time.deltaTime * speed);
        if (charging && currentTimer.IsOn())
        {
            Color currentColor = spriteRenderer.color;
            float percentage = launchController.GetChargeAmount();
            float diff = 1 - OGColor.g;

            Color color = new Color(currentColor.r, currentColor.g + diff*percentage, currentColor.b);
            spriteRenderer.color = color;
            trailRenderer.startColor = color;
            trailRenderer.endColor = color;
        }
    }
}

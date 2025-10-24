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
    PlayerAbilityController pbc;
    Color OGColor;
    SpriteRenderer spriteRenderer;
    TrailRenderer trailRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        OGColor = spriteRenderer.color;
        pbc = FindObjectOfType<PlayerAbilityController>();
        pbc.OnBeginCharge += ChargeHappening;

        RedOrbController redOrb = FindObjectOfType<RedOrbController>();
        redOrb.OnRemoveChargeThrown += ChargeRemoved;

        PlayerAbilityController playerAbilityController = FindObjectOfType<PlayerAbilityController>();
        playerAbilityController.OnSwapBegin += Shrink;
        playerAbilityController.OnSwapEnd += Grow;
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
        OGSize = transform.localScale;
        shrinkin = true;
    }

    void Grow()
    {
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
                shrinkin = false;
            }
        }
        else if (growing)
        {
            if (growTimer.TimerStillGoing(Time.time))
            {
                float percentage = growTimer.percentageComplete(Time.time);
                transform.localScale = (OGSize * percentage);
            }
            else
            {
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

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector2.Lerp(TrackObject.transform.position, transform.position, Time.deltaTime * speed);
        if (charging && currentTimer.IsOn())
        {
            Color currentColor = spriteRenderer.color;
            float percentage = pbc.GetChargeAmount();
            float diff = 1 - OGColor.g;

            Color color = new Color(currentColor.r, currentColor.g + diff*percentage, currentColor.b);
            spriteRenderer.color = color;
            trailRenderer.startColor = color;
            trailRenderer.endColor = color;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public Transform Player;
    public Camera camera;              // Reference to the player's transform
    public Vector2 parallaxEffectMultiplier; // Controls how much the background moves

    Material material;
    float startSize;
    float OGScale;

    Vector2 OGOffset;
    Vector2 OgCamPos;

    float OgRandomness;

    float RandomDirection = 1f;

    void Start()
    {

        startSize = camera.orthographicSize;
        material = GetComponent<SpriteRenderer>().material;

        OGScale = material.GetFloat("_OverallScale");
        OGOffset = material.GetVector("_Offset");
        OgRandomness = material.GetFloat("_Randomness");

        OgCamPos = camera.transform.position;
    }

    void Update()
    {
        if(Player != null)
        {
            transform.position = Player.transform.position;
            float factor = startSize / camera.orthographicSize;
            material.SetFloat("_OverallScale", factor * OGScale);

            Vector2 newDiff = (Vector2)camera.transform.position - OgCamPos;
            material.SetVector("_Offset", newDiff * parallaxEffectMultiplier);

        }

        float currentRandomness = material.GetFloat("_Randomness");
        if(currentRandomness >= 5f || (currentRandomness <= 0))
        {
            RandomDirection = -RandomDirection;
        }

        material.SetFloat("_Randomness", currentRandomness + (Time.deltaTime * parallaxEffectMultiplier.x * RandomDirection));


    }
}

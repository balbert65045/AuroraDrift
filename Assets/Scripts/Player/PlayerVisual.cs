using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    public GameObject TrackObject;
    [SerializeField] float speed = 10f;

    //private void Start()
    //{
    //    //rb
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

    public void SetTrackObject(GameObject trackObject, Rigidbody2D rb)
    {
        TrackObject = trackObject;
        GetComponent<PlayerSquishController>().rb = rb;
    }

    public void SetTrail(bool value)
    {
        //if (value)
        //{
        //    GetComponent<TrailRenderer>().enabled = true;
        //}
        //else
        //{
        //    GetComponent<TrailRenderer>().enabled = false;
        //}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector2.Lerp(TrackObject.transform.position, transform.position, Time.deltaTime * speed);
    }
}

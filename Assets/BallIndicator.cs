using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallIndicator : MonoBehaviour
{
    [Header("Scene References")]
    public Transform target;                // What we’re pointing to
    public Transform player;                // Used only if you want distance-based effects (optional)
    public Camera cam;                      // Your main camera
    public RectTransform arrowRect;

    public float edgePadding = 40f;         // Keep arrow away from screen edges
    public bool hideWhenOnscreen = true;    // Hide arrow if target is visible
    public bool faceFromScreenCenter = true;// Rotate arrow from screen center (recommended)

    Canvas _canvas;
    RectTransform _canvasRect;
    // Start is called before the first frame update
    void Start()
    {
        if(!cam) cam = Camera.main;
        _canvas = arrowRect.GetComponentInParent<Canvas>();
        _canvasRect = _canvas.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || cam == null || arrowRect == null || _canvasRect == null) return;

        // World -> Screen
        Vector3 sp = cam.WorldToScreenPoint(target.position);
        bool behind = sp.z < 0f;

        // On-screen check (2D top-down orthographic: z usually > 0)
        bool onScreen = !behind &&
                        sp.x >= 0f && sp.x <= Screen.width &&
                        sp.y >= 0f && sp.y <= Screen.height;

        if (onScreen && hideWhenOnscreen)
        {
            if (arrowRect.gameObject.activeSelf) arrowRect.gameObject.SetActive(false);
            return;
        }

        if (!arrowRect.gameObject.activeSelf) arrowRect.gameObject.SetActive(true);

        // If target is behind camera, mirror to keep direction sensible
        if (behind)
        {
            sp.x = Screen.width - sp.x;
            sp.y = Screen.height - sp.y;
        }

        // Clamp to screen edge with padding
        sp.x = Mathf.Clamp(sp.x, edgePadding, Screen.width - edgePadding);
        sp.y = Mathf.Clamp(sp.y, edgePadding, Screen.height - edgePadding);

        // Screen -> Canvas local
        Vector2 localPos;
        Camera uiCam = _canvas.renderMode == RenderMode.ScreenSpaceCamera ? _canvas.worldCamera : null;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, sp, uiCam, out localPos);
        arrowRect.anchoredPosition = localPos;

        // Rotate arrow
        Vector2 centerScreen = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector2 dirScreen = faceFromScreenCenter
            ? (new Vector2(sp.x, sp.y) - centerScreen)
            : Vector2.right; // fallback

        float angle = Mathf.Atan2(dirScreen.y, dirScreen.x) * Mathf.Rad2Deg;
        arrowRect.rotation = Quaternion.AngleAxis(angle -90, Vector3.forward);
    }
}

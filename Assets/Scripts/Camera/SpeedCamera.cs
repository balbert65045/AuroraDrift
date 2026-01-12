using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class SpeedCamera : MonoBehaviour
{
    PlayerOrbitController orbitController;
    [SerializeField] PlayerMovement pm;
    [SerializeField] float zoomSpeed = 20;
    CinemachineVirtualCamera cam;
    float minSize;


    CinemachineFramingTransposer transposer;
    // Start is called before the first frame update
    void Start()
    {
        orbitController = pm.GetComponent<PlayerOrbitController>();
        cam = GetComponent<CinemachineVirtualCamera>();
        transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
        minSize = transposer.m_MinimumOrthoSize;
    }


    public void SetNewTarget(Transform newTarget)
    {
        cam.Follow = newTarget;
        StartCoroutine("IncreaseCamSize");
    }

    IEnumerator IncreaseCamSize()
    {
        while (cam.m_Lens.OrthographicSize < 100)
        {
            cam.m_Lens.OrthographicSize = cam.m_Lens.OrthographicSize + Time.deltaTime * 20f;
            yield return new WaitForEndOfFrame();
        }
    }

    bool InControl = true;
    TimerClass ZoomOutTimer = new TimerClass(false);
    float ZoomOutTime = 2f;
    float maxZoomOut = 100f;
    float minZoomOut = 50f;
    public void TakeControl()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
        minSize = transposer.m_MinimumOrthoSize;
        InControl = false;
        ZoomOutTimer = new TimerClass(true, ZoomOutTime, Time.time);
        transposer.m_MinimumOrthoSize = minZoomOut;
    }

    public void ReleaseControl()
    {
        InControl = true;
        transposer.m_MinimumOrthoSize = minSize;

    }

    // Update is called once per frame
    void Update()
    {
        if (!InControl) {
            if (ZoomOutTimer.IsOn())
            {
                if (ZoomOutTimer.TimerStillGoing(Time.time))
                {
                    float percentage = ZoomOutTimer.percentageComplete(Time.time);
                    transposer.m_MinimumOrthoSize = minZoomOut + (percentage * (maxZoomOut - minZoomOut));
                }
            }

            return;
        }
        //float increase = Mathf.Clamp(pm.GetComponent<Rigidbody2D>().velocity.magnitude / pm.maxSpeed, 1, 100);
        //float newSize = increase * minSize;
        //transposer.m_MinimumOrthoSize = Mathf.Lerp(transposer.m_MinimumOrthoSize, newSize, Time.deltaTime * zoomSpeed);

        if (pm.dashing) { return; }
        if (pm.pulling || orbitController.Orbiting)
        {
            float increase = Mathf.Clamp(pm.GetComponent<Rigidbody2D>().velocity.magnitude / pm.GetMaxSpeed(), 1, 100);
            float newSize = increase * minSize;
            transposer.m_MinimumOrthoSize = Mathf.Lerp(transposer.m_MinimumOrthoSize, newSize, Time.deltaTime * zoomSpeed);
        }
        else
        {

            float increase = Mathf.Clamp(pm.GetComponent<Rigidbody2D>().velocity.magnitude / pm.GetMaxSpeed(), 1, 100);
            float newSize = Mathf.Max(increase/2, 1) * minSize;
            transposer.m_MinimumOrthoSize = Mathf.Lerp(transposer.m_MinimumOrthoSize, newSize, Time.deltaTime * zoomSpeed);

        }

    }
}

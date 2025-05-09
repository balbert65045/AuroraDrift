using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [Header("Input Axes")]
    [Tooltip("Name of the horizontal input axis.")]
    public string horizontalAxis = "Horizontal";
    [Tooltip("Name of the vertical input axis.")]
    public string verticalAxis = "Vertical";

    PlayerMovement pm;
    PlayerRotationController rotationController;
    PlayerPullController pullController;

    [SerializeField] bool UseController = true;

    public bool GetHoldingDown()
    {
        return (Input.GetAxisRaw("PullRed") == 1 || Input.GetAxis("PullBlue") == 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        rotationController = FindObjectOfType<PlayerRotationController>();
        pullController = FindObjectOfType<PlayerPullController>();
    }

    bool pullingRed = false;
    bool pullingBlue = false;
    // Update is called once per frame
    void Update()
    {
        //MOVEMENT//
        float inputX = Input.GetAxisRaw(horizontalAxis);
        float inputY = Input.GetAxisRaw(verticalAxis);
        DoMovement(inputX, inputY);

        if (Input.GetButtonDown("Dash"))
        {
            DoDash();
        }
        //

        //ROTATION//
        if(UseController)
        {
            //float inputXLook = Input.GetAxisRaw("LookDirHor");
            //float inputYLook = Input.GetAxisRaw("LookDirVert");
            //if((new Vector2(inputX, inputY)).magnitude > .4f)
            //{
            //    DoLookDirection(new Vector2(inputX, inputY));
            //}
            DoLookDirection(new Vector2(inputX, inputY));
        }
        else
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0f; // Ensure we are only rotating in 2D

            // Calculate the direction from the object's position to the mouse position
            Vector3 direction = mouseWorldPosition - rotationController.gameObject.transform.position;
            DoLookDirection(direction);
        }

        //

        //PushPull//
        if (UseController)
        {
            float PullRedAxis = Input.GetAxisRaw("PullRed");
            float PullBlueAxis = Input.GetAxisRaw("PullBlue");

            if (PullRedAxis > .5f && !pullingRed)
            {
                pullingRed = true;
                DoPullRed();
            }
            else if (pullingRed && PullRedAxis == 0)
            {
                pullingRed = false;
                DoStopPullRed();
            }

            if(PullBlueAxis > .5f && !pullingBlue)
            {
                pullingBlue = true;
                DoPullBlue();
            }
            else if(pullingBlue && PullBlueAxis == 0)
            {
                pullingBlue = false;
                DoStopPullBlue();
            }
        }
        else
        {
            if (Input.GetButtonDown("PullRed"))
            {
                DoPullRed();
            }
            else if (Input.GetButtonUp("PullRed")) { DoStopPullRed(); }

            if (Input.GetButtonDown("PullBlue"))
            {
                DoPullBlue();
            }
            else if (Input.GetButtonUp("PullBlue")) { DoStopPullBlue(); }
        }
        //
    }

    void DoPullRed() {
        pullController.ReceivePullRed();
    }

    void DoStopPullRed()
    {
        pullController.ReceiveThrow_StopPullRed();

    }

    void DoPullBlue()
    {
        pullController.RecivePullBlue();
    }

    void DoStopPullBlue()
    {
        pullController.ReceiveThrow_StopPullBlue();
    }

    void DoLookDirection(Vector2 dir)
    {
        rotationController.ReceiveLookDir(dir);
    }
    void DoDash()
    {
        pm.ReceiveDash();
    }

    void DoMovement(float x, float y)
    {
        pm.ReceiveMovment(x, y);
    }
}

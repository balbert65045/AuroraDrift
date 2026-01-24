using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class SwingController : MonoBehaviour
{
    public Action<bool> OnSwingBegin;
    public Action OnSwingEndRed;
    public Action OnSwingEndBlue;


    SwingArea BlueSwingArea;
    PolygonCollider2D BlueCollider;

    SwingArea RedSwingArea;
    PolygonCollider2D RedCollider;

    PlayerMovement pm;
    PlayerOrbitController playerOrbitController;
    RedOrbController redOrbController;
    RedOrbCollision redOrbCollision;

    PlayerCollisionController playerCollision;

    bool Swinging = false;

    TargetGroupController targetGroupController;

    ButtonRegion RedRegion;
    ButtonRegion BlueRegion;

    public void Reconnect()
    {
        pm = FindObjectOfType<PlayerMovement>();
        redOrbController = FindObjectOfType<RedOrbController>();
        redOrbCollision = FindObjectOfType<RedOrbCollision>();
        PlayerInputController input = FindObjectOfType<PlayerInputController>();
        playerCollision = FindObjectOfType<PlayerCollisionController>();
        targetGroupController = FindObjectOfType<TargetGroupController>();

        SetupInput();
        playerOrbitController = FindObjectOfType<PlayerOrbitController>();

        BlueOrbStateController stateController = FindObjectOfType<BlueOrbStateController>();
        RedOrbStateController redStateController = FindObjectOfType<RedOrbStateController>();
        stateController.OnEnterBlackHole += EndSwingViaBlackHole;
        if(redStateController != null)
        {
            redStateController.OnEnterBlackHole += EndSwingViaBlackHole;
        }

        //input.OnSkill2Input += DoSwingRed;
        //input.OnSkill2Release += EndSwing;

        //input.OnSkill3Input += DoSwingBlue;
        //input.OnSkill3Release += EndSwing;

        //playerCollision.OnDealDamage += EndSwing;
        //redOrbCollision.OnDealDamage += EndSwing;
        SwingCollider[] SwingColliders = FindObjectsOfType<SwingCollider>();
        foreach(SwingCollider swingcol in SwingColliders)
        {
            if (swingcol.IsBlue)
            {
                BlueCollider = swingcol.GetComponent<PolygonCollider2D>();
            }
            else
            {
                RedCollider = swingcol.GetComponent<PolygonCollider2D>();
            }
        }

        SwingArea[] swingAreas = FindObjectsOfType<SwingArea>();
        foreach(SwingArea swingArea in swingAreas)
        {
            if (swingArea.IsBlue)
            {
                BlueSwingArea = swingArea;
            }
            else
            {
                RedSwingArea = swingArea;
            }
        }
    }

    void EndSwingViaBlackHole(Transform _t1, Transform _t2)
    {
        EndSwing();
    }

    bool SwingEnabled = false;

    [SerializeField] Upgrade currentRedAbility;
    [SerializeField] Upgrade currentBlueAbility;


    public float RedLineDamage = 10;
    public float BlueLineDamage = 10;

    public void SetAbility(Upgrade ability)
    {
        bool firstSetup = false;
        if(ability.abilityType == AbilityType.RedSwing)
        {
            firstSetup = currentRedAbility == null;
            currentRedAbility = ability;
            RedLineDamage = ability.GetTotalAmountCalculated();
            if (firstSetup)
            {
                Debug.Log("SettingUpInput");
                RedRegion = currentRedAbility.region;
                SetupInput();
            }
        }
        else if(ability.abilityType == AbilityType.BlueSwing)
        {
            firstSetup = currentBlueAbility == null;
            currentBlueAbility = ability;
            BlueLineDamage = ability.GetTotalAmountCalculated();
            if (firstSetup)
            {
                Debug.Log("SettingUpInput");
                BlueRegion = currentBlueAbility.region;
                SetupInput();
            }
        }
    }

    void SetupInput()
    {
        PlayerInputController inputController = FindObjectOfType<PlayerInputController>();
        
        if (RedRegion == ButtonRegion.Ability2)
        {
            inputController.OnSkill2Input += DoSwingRed;
            inputController.OnSkill2Release += EndSwing;
        }
        else if (RedRegion == ButtonRegion.Ability3)
        {
            inputController.OnSkill3Input += DoSwingRed;
            inputController.OnSkill3Release += EndSwing;
        }

        if (BlueRegion == ButtonRegion.Ability2)
        {
            inputController.OnSkill2Input += DoSwingBlue;
            inputController.OnSkill2Release += EndSwing;
        }
        else if (BlueRegion == ButtonRegion.Ability3)
        {
            inputController.OnSkill3Input += DoSwingBlue;
            inputController.OnSkill3Release += EndSwing;
        }
    }

    public void ResetValues()
    {
        currentRedAbility = null;
        currentBlueAbility = null;
    }

    bool attemptingToSwingRed = false;


    void DoSwingRed()
    {
        if (currentRedAbility == null) { return; }
        if (playerOrbitController.Orbiting)
        {
            return;
        }
        attemptingToSwingRed = true;
        if (!RedSwingArea.PlayerInSwingRange())
        {
            RedSwingArea.ShowRange();
            return;
        }
    }


    bool attemptingToSwingBlue = false;
    bool showingRange = false;
    void DoSwingBlue()
    {
        if (currentBlueAbility == null) { return; }
        if (playerOrbitController.Orbiting)
        {
            return;
        }
        attemptingToSwingBlue = true;
        if (!BlueSwingArea.PlayerInSwingRange()) {
            BlueSwingArea.ShowRange();
            return;
        }
    }

    void EndSwing()
    {
        bool blue = attemptingToSwingBlue;

        targetGroupController.AddBlueOrbWeight();
        targetGroupController.AddRedOrbWeight();

        if(redOrbCollision != null)
        {
            redOrbCollision.EnableCollision();
        }
        playerCollision.EnableCollision();

        attemptingToSwingBlue = false;
        attemptingToSwingRed = false;

        Swinging = false;
        BlueSwingArea.HideRange();
        RedSwingArea.HideRange();
        Debug.Log("End Swing");
        pm.EndSwing();
        BlueCollider.enabled = false;
        RedCollider.enabled = false;
        if(redOrbController != null)
        {
            redOrbController.StopSwing();
        }
        if (blue)
        {
            if (OnSwingEndBlue != null) { OnSwingEndBlue.Invoke(); }
        }
        else
        {
            if(OnSwingEndRed != null) { OnSwingEndRed.Invoke(); }
        }
    }

    void Update() {
        if (attemptingToSwingBlue)
        {
            if (Swinging) { return; }
            if (BlueSwingArea.PlayerInSwingRange())
            {
                if (!Swinging)
                {
                    StartSwingBlue();
                }
                else
                {
                    BlueSwingArea.ShowRange();
                }
            }
        }

        if (attemptingToSwingRed)
        {
            if(Swinging) { return; }
            if (RedSwingArea.PlayerInSwingRange())
            {
                if (!Swinging)
                {
                    StartSwingRed();
                }
                else
                {
                    RedSwingArea.ShowRange();
                }
            }
        }
    }

    void StartSwingRed()
    {
        targetGroupController.RemoveRedOrbWeight();

        redOrbCollision.DisableCollision();
        blueSwing = false;

        RedSwingArea.HideRange();
        Swinging = true;

        pm.BeginSwingRed();
        redOrbController.BeginSwingRed(pm.transform);
        RedCollider.enabled = true;
        if (OnSwingBegin != null) { OnSwingBegin.Invoke(false); }
    }

    void StartSwingBlue()
    {
        targetGroupController.RemoveBlueOrbWeight();
        playerCollision.DisableCollision();

        blueSwing = true;
        BlueSwingArea.HideRange();
        Swinging = true;

        pm.BeginSwingBlue(redOrbController.transform);
        redOrbController.BeginSwingBlue();
        BlueCollider.enabled = true;
        if (OnSwingBegin != null) { OnSwingBegin.Invoke(true); }
    }

    bool blueSwing = false;

    Vector2 PlayerPrevPos;
    Vector2 RedOrbPrevPos;
    private void FixedUpdate()
    {
        if (Swinging)
        {
            if (blueSwing)
            {
                Vector2[] points = new Vector2[]
                {
                                redOrbController.transform.position,
                                PlayerPrevPos,
                                pm.transform.position
                };
                BlueCollider.points = points;
            }
            else
            {
               Vector2[] points = new Vector2[]
                {
                               pm.transform.position,
                               RedOrbPrevPos,
                               redOrbController.transform.position
                };
                RedCollider.points = points;
            }
        }
        if(redOrbController == null) { return; }
        PlayerPrevPos = pm.transform.position;
        RedOrbPrevPos = redOrbController.transform.position;
    }

}

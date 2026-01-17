using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGroupController : MonoBehaviour
{
    [SerializeField] RedOrbController redOrbController;
    [SerializeField] float MaxDistanceAway = 50;
    

    [SerializeField] CinemachineTargetGroup targetGroup;
    [SerializeField] float adjustSpeed = 1f;

    [SerializeField] GameObject Player;

    List<Transform> newMembers = new List<Transform>();
    List<Transform> removingMembers = new List<Transform>();

    int RedOrbIndex;
    RedOrbController redOrb;
    [SerializeField] float MinMaxRedOrbRange = 100f;
    [SerializeField] float MaxMaxRedOrbRange = 120f;

    bool showingRedOrb = false;
    void AdjustRedOrb()
    {
        if (redOrb == null || showingRedOrb) { return; }
        float diff = (redOrb.transform.position - Player.transform.position).magnitude;
        float weight;
        if (diff < MinMaxRedOrbRange)
        {
            weight =  .7f;
        }
        else if(diff > MinMaxRedOrbRange && diff < MaxMaxRedOrbRange)
        {
            float percentage = 1 - (diff - MinMaxRedOrbRange) / (MaxMaxRedOrbRange - MinMaxRedOrbRange);
            weight = percentage * .7f;
        }
        else
        {
            weight = 0f;
        }
        targetGroup.m_Targets[RedOrbIndex].weight = weight;

    }
    private void Update()
    {
        if (!InControl) { return; }
        AdjustRedOrb();
        //float distance = (redOrbController.transform.position - Player.transform.position).magnitude;
        //if (distance > MaxDistanceAway)
        //{
        //    int index = targetGroup.FindMember(redOrbController.transform);
        //    float percentage = (distance - MaxDistanceAway) / MaxDistanceAway;
        //    targetGroup.m_Targets[index].weight = .7f - (.7f * percentage);
        //}
        //else
        //{
        //    int index = targetGroup.FindMember(redOrbController.transform);
        //    targetGroup.m_Targets[index].weight = .7f;
        //}

        transform.position = Player.transform.position;
        if(newMembers.Count > 0)
        {
            List<Transform> finished = new List<Transform>();
            foreach(Transform t in newMembers)
            {
                int index = targetGroup.FindMember(t);
                targetGroup.m_Targets[index].weight = Mathf.Clamp01(targetGroup.m_Targets[index].weight + Time.deltaTime * adjustSpeed);
                if(targetGroup.m_Targets[index].weight >= 1f)
                {
                    finished.Add(t);
                }
            }

            foreach(Transform t in finished)
            {
                newMembers.Remove(t);
            }
        }

        if (removingMembers.Count > 0)
        {
            List<Transform> finished = new List<Transform>();
            foreach (Transform t in removingMembers)
            {
                int index = targetGroup.FindMember(t);
                if(index == -1)
                {
                    finished.Add(t);
                }
                else
                {
                    targetGroup.m_Targets[index].weight = Mathf.Clamp01(targetGroup.m_Targets[index].weight - Time.deltaTime * adjustSpeed);
                    if (targetGroup.m_Targets[index].weight <= 0)
                    {
                        finished.Add(t);
                    }
                }
            }

            foreach (Transform t in finished)
            {

                targetGroup.RemoveMember(t);

                removingMembers.Remove(t);
                RecalculateRedOrbIndex();
            }
        }
    }

    void RecalculateRedOrbIndex()
    {
        if(redOrb == null) { return; }
        RedOrbIndex = targetGroup.FindMember(redOrb.transform);
    }

    private void Start()
    {
        redOrb = FindAnyObjectByType<RedOrbController>();
        if(redOrb != null)
        {
            RedOrbIndex = targetGroup.FindMember(redOrb.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ship>() || collision.GetComponent<TutorialBlock>() || collision.GetComponent<RedOrbCamTracker>() || collision.transform.tag == "Enemy")
        {
            AddNewMember(collision.transform);
            //targetGroup.AddMember(collision.transform, 0, 5f);
            //newMembers.Add(collision.transform);
            //if (removingMembers.Contains(collision.transform))
            //{
            //    removingMembers.Remove(collision.transform);
            //}
        }

        if(redOrb == null)
        {
            if (collision.GetComponent<RedOrbController>())
            {
                redOrb = collision.GetComponent<RedOrbController>();
                targetGroup.AddMember(redOrb.transform, 0f, 10);

                RedOrbIndex = targetGroup.FindMember(redOrb.transform);
                showingRedOrb = true;
                StartCoroutine("IncreaseRedOrbWeght");
            }
        }
    }

    IEnumerator IncreaseRedOrbWeght()
    {
        while(targetGroup.m_Targets[RedOrbIndex].weight < .7f)
        {
            yield return new WaitForEndOfFrame();
            targetGroup.m_Targets[RedOrbIndex].weight += Time.deltaTime;
        }
        showingRedOrb = false;
    }

    public void AddNewMember(Transform member)
    {
        targetGroup.AddMember(member, 0, 5f);
        newMembers.Add(member);
        if (removingMembers.Contains(member))
        {
            removingMembers.Remove(member);
        }
    }

    public void RemoveNewMember(Transform member)
    {
        removingMembers.Add(member);
        if (newMembers.Contains(member))
        {
            newMembers.Remove(member);
        }
    }

    CinemachineTargetGroup.Target[] previousTargets;
    bool InControl = true;
    public void TakeControl(Transform newTarget)
    {
        GetComponent<CircleCollider2D>().enabled = false;
        InControl = false;
        previousTargets = targetGroup.m_Targets;
        foreach(CinemachineTargetGroup.Target target in previousTargets)
        {
            targetGroup.RemoveMember(target.target);
        }
        targetGroup.AddMember(newTarget, 1, 10);
    }

    public void ReleaseControl()
    {
        GetComponent<CircleCollider2D>().enabled = true;
        InControl = true;
        CinemachineTargetGroup.Target[] currentTargets = targetGroup.m_Targets;
        //foreach (CinemachineTargetGroup.Target target in currentTargets)
        //{
        //    targetGroup.RemoveMember(target.target);
        //}

        foreach (CinemachineTargetGroup.Target target in previousTargets)
        {
            targetGroup.AddMember(target.target, 0, target.radius);
        }

        StartCoroutine("IncreaseDecreaseWeigths");
    }

    IEnumerator IncreaseDecreaseWeigths()
    {
        CinemachineTargetGroup.Target BossTarget = targetGroup.m_Targets[0];
        CinemachineTargetGroup.Target Player = targetGroup.m_Targets[1];
        CinemachineTargetGroup.Target Red = targetGroup.m_Targets[2];

        while(targetGroup.m_Targets[0].weight > .65f || targetGroup.m_Targets[1].weight < 1 || targetGroup.m_Targets[2].weight < .7f)
        {
            if(targetGroup.m_Targets[0].weight > .65f)
            {
                targetGroup.m_Targets[0].weight -= Time.deltaTime;
            }
            if(targetGroup.m_Targets[1].weight  < 1)
            {
                targetGroup.m_Targets[1].weight += Time.deltaTime;
            }
            if(targetGroup.m_Targets[2].weight < .7f)
            {
                targetGroup.m_Targets[2].weight += Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();
        }

        //targetGroup.RemoveMember(BossTarget.target);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Ship>() || collision.GetComponent<TutorialBlock>() || collision.GetComponent<RedOrbCamTracker>() || collision.transform.tag == "Enemy")
        {
            RemoveNewMember(collision.transform);

            //removingMembers.Add(collision.transform);
            //if (newMembers.Contains(collision.transform))
            //{
            //    newMembers.Remove(collision.transform);
            //}
        }
    }
}

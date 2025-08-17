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
    private void Update()
    {
        float distance = (redOrbController.transform.position - Player.transform.position).magnitude;
        if (distance > MaxDistanceAway)
        {
            int index = targetGroup.FindMember(redOrbController.transform);
            float percentage = (distance - MaxDistanceAway) / MaxDistanceAway;
            targetGroup.m_Targets[index].weight = .7f - (.7f * percentage);
        }
        else
        {
            int index = targetGroup.FindMember(redOrbController.transform);
            targetGroup.m_Targets[index].weight = .7f;
        }

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
                targetGroup.m_Targets[index].weight = Mathf.Clamp01(targetGroup.m_Targets[index].weight - Time.deltaTime * adjustSpeed);
                if (targetGroup.m_Targets[index].weight <= 0)
                {
                    finished.Add(t);
                }
            }

            foreach (Transform t in finished)
            {
                targetGroup.RemoveMember(t);
                removingMembers.Remove(t);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ship>())
        {
            targetGroup.AddMember(collision.transform, 0, 5f);
            newMembers.Add(collision.transform);
            if (removingMembers.Contains(collision.transform))
            {
                removingMembers.Remove(collision.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Ship>())
        {
            removingMembers.Add(collision.transform);
            if (newMembers.Contains(collision.transform))
            {
                newMembers.Remove(collision.transform);
            }
        }
    }
}

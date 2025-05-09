using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGroupController : MonoBehaviour
{
    [SerializeField] CinemachineTargetGroup targetGroup;
    [SerializeField] float adjustSpeed = 1f;

    public List<int> gateAddedIndex = new List<int>();


    private void Update()
    {
        if(gateAddedIndex.Count > 0)
        {
            foreach(int gateIndex in gateAddedIndex)
            {
                targetGroup.m_Targets[gateIndex].weight += Time.deltaTime * adjustSpeed;
                if (targetGroup.m_Targets[gateIndex].weight >= 1)
                {
                    targetGroup.m_Targets[gateIndex].weight = 1;
                    gateAddedIndex.Remove(gateIndex);
                }
            }
        }
    }
}

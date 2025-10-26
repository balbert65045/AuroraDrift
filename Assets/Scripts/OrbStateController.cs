using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbStateController : MonoBehaviour
{
    bool InBlackHole = false;


    public Action<Transform, Transform> OnEnterBlackHole;
    public Action OnExitBlackHole;
    public Action<float> OnShrink;

    public void Shrink(float time)
    {
        if(OnShrink != null) OnShrink(time);
    }

    public void EnterBlackHole(Transform trackPos, Transform BlackHolePos)
    {
        InBlackHole = true;
        if (OnEnterBlackHole != null) OnEnterBlackHole(trackPos, BlackHolePos);
    }

    public void ExitBlackHole()
    {
        InBlackHole = false;
        if (OnExitBlackHole != null) OnExitBlackHole();
    }
}

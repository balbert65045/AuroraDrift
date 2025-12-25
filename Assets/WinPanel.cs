using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    public void Win()
    {
        Panel.SetActive(true);
    }
}

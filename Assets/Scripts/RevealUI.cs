using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RevealUI : MonoBehaviour
{
    [SerializeField] GameObject FirstSelectObj;
    [SerializeField] float Delay = 0f;
    private void OnEnable()
    {
        StartCoroutine("WaitAndThenSelect");
    }

    IEnumerator WaitAndThenSelect()
    {
        yield return new WaitForSeconds(Delay);
        FindObjectOfType<EventSystem>().firstSelectedGameObject = FirstSelectObj;
        FindObjectOfType<EventSystem>().SetSelectedGameObject(FirstSelectObj);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int ScoreToBeat = 30000;
    public bool finishingLevel = false;

    [SerializeField] GameObject PlayerVisual;
    [SerializeField] GameObject SwordVisual;
    [SerializeField] GameObject BlackHoldePrefab;
    [SerializeField] GameObject WinCanvas;
    public void CompleteLevel()
    {
        if (finishingLevel) { return; }
        finishingLevel = true;
        StartCoroutine("DoLevelFinish");
    }

    IEnumerator DoLevelFinish()
    {


        PlayerVisual.GetComponent<PlayerSquishController>().enabled = false;
        SwordVisual.GetComponent<PlayerSquishController>().enabled = false;

        
        PlayerVisual.transform.localScale = new Vector3(0, 0, 1);
        SwordVisual.transform.localScale = new Vector3(0, 0, 1);


        PlayerVisual.GetComponent<TrailRenderer>().emitting = false;
        SwordVisual.GetComponent<TrailRenderer>().emitting = false;

        Vector3 pos = new Vector3((PlayerVisual.transform.position.x + SwordVisual.transform.position.x) / 2, (PlayerVisual.transform.position.y + SwordVisual.transform.position.y) / 2, 0);
        GameObject blockHoleInstance = Instantiate(BlackHoldePrefab, pos, Quaternion.identity);
        blockHoleInstance.transform.localScale = Vector3.zero;

        FindObjectOfType<SpeedCamera>().SetNewTarget(blockHoleInstance.transform);
        while (blockHoleInstance.transform.localScale.x < 80)
        {
            blockHoleInstance.transform.localScale = new Vector3(blockHoleInstance.transform.localScale.x + Time.deltaTime * 20, blockHoleInstance.transform.localScale.y + Time.deltaTime * 20, 1);
            yield return new WaitForEndOfFrame();

        }
        WinCanvas.SetActive(true);
        WinCanvas.transform.position = blockHoleInstance.transform.position;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

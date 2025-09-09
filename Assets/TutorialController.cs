using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    int currentIndex = 0;
    [SerializeField] GameObject Ship;
    [SerializeField] GameObject[] TutorialPompts;
    [SerializeField] GameObject Previous;
    [SerializeField] GameObject Next;
    [SerializeField] GameObject RedOrbController;
    [SerializeField] GameObject RedOrbVisual;

    [SerializeField] GameObject TutorialBlockPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Previous.SetActive(false);
        //StartCoroutine("DoStartAnimation");
    }

    IEnumerator DoStartAnimation()
    {
        Next.transform.localScale = Vector3.zero;
        Previous.transform.localScale = Vector3.zero;
        TutorialPompts[0].transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(.2f);
        while(Next.transform.localScale.x < 1)
        {
            Next.transform.localScale += Vector3.one * Time.deltaTime * 2f;
            Previous.transform.localScale += Vector3.one * Time.deltaTime * 2f;
            TutorialPompts[0].transform.localScale += Vector3.one * Time.deltaTime * 2f;
            yield return new WaitForEndOfFrame();
        }
        Next.transform.localScale = Vector3.one;
        Previous.transform.localScale = Vector3.one;
        TutorialPompts[0].transform.localScale = Vector3.one;
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Next"))
        {
            if (currentIndex == TutorialPompts.Length - 1) { return; }
            if (!Next.activeSelf) { return; }
            Previous.SetActive(true);

            TutorialPompts[currentIndex].SetActive(false);
            currentIndex++;
            TutorialPompts[currentIndex].SetActive(true);
            if (currentIndex == TutorialPompts.Length - 1)
            {
                Next.SetActive(false);
            }
            CheckForAction();
        }
        else if (Input.GetButtonDown("Previous"))
        {
            if (currentIndex == 0) { return; }
            if (!Previous.activeSelf) { return; }
            Next.SetActive(true);

            TutorialPompts[currentIndex].SetActive(false);
            currentIndex--;
            TutorialPompts[currentIndex].SetActive(true);
            if (currentIndex == 0)
            {
                Previous.SetActive(false);
            }
        }
    }


    void CheckForAction()
    {
        if(currentIndex == 2)
        {
            //Reveal Red Orb
            if (!RedOrbController.activeSelf)
            {
                RedOrbController.SetActive(true);
                RedOrbVisual.SetActive(true);
                PlayerMovement pm = FindObjectOfType<PlayerMovement>();
                Vector2 pos = (Vector2)pm.transform.position + Vector2.right * 70;
                RedOrbVisual.transform.position = pos;
                RedOrbController.transform.position = pos;
            }
        }
        if (currentIndex == 5)
        {
            if (!Ship.activeSelf)
            {
                Ship.SetActive(true);
                PlayerMovement pm = FindObjectOfType<PlayerMovement>();
                Vector2 pos = (Vector2)pm.transform.position + Vector2.right * 70;
                Ship.transform.position = pos;
            }
        }
    }

    public void ShowNextTutorial(Vector2 pos)
    {
        currentIndex++;
        CheckForAction();
        Vector2 newPos = pos + Vector2.up * 2f + Vector2.right * 25f;
        switch (currentIndex)
        {
            //case 1:
        }
        TutorialPompts[currentIndex].transform.position = newPos;
        TutorialPompts[currentIndex].SetActive(true);
    }

    public void ShowNextBlock(Vector2 pos)
    {
        if(currentIndex == TutorialPompts.Length - 1) { return; }
        TutorialPompts[currentIndex-1].SetActive(false);
        Vector2 posOffset = pos + Vector2.right * 50;
        switch (currentIndex)
        {
            case 2:
                posOffset = pos + Vector2.right * 25 + Vector2.up * 25;
                break;

        }
        Instantiate(TutorialBlockPrefab, posOffset, Quaternion.identity);
    }

}

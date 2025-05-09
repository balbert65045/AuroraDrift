using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Score : MonoBehaviour
{
    [SerializeField] float Tier2Speed = 10f;
    [SerializeField] float Tier3Speed = 20f;
    [SerializeField] float Tier4Speed = 30f;
    [SerializeField] float Tier5Speed = 40f;


    [SerializeField] TMP_Text MultText;
    [SerializeField] TMP_Text CurrentScoreText;
    [SerializeField] TMP_Text AddedScoreText;

    [SerializeField] float TimeShowScore = .4f;
    [SerializeField] float TimeHoldMult = 4f;
    [SerializeField] float scoreToSpeedRatio = .02f;



    int currentScore = 0;
    int currentMult = 1;

    bool scoreAdded = false;
    float timeScoreAdded;

    int enemysKilledInCombo = 0;

    GameManager manager;
    PlayerPullController playerPullController;
    float baseSpeed;
    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
        playerPullController = FindObjectOfType<PlayerPullController>();
        baseSpeed = playerPullController.PushPullSpeed;
    }

    public void AddScore(int score)
    {
        Color multColor = Color.yellow;
        //Super Simple method
        playerPullController.AdjustPushPullSpeed(playerPullController.PushPullSpeed + score * scoreToSpeedRatio);

        //enemysKilledInCombo++;
        //if (enemysKilledInCombo > 30)
        //{
        //    MultText.gameObject.SetActive(true);

        //    currentMult = 5;
        //    multColor = Color.magenta;
        //    playerPullController.AdjustPushPullSpeed(baseSpeed + Tier5Speed);
        //    playerPullController.GetComponent<PlayerLineController>().UpdateLineState(5);
        //}
        //else if(enemysKilledInCombo > 20)
        //{
        //    MultText.gameObject.SetActive(true);

        //    currentMult = 4;
        //    multColor = Color.blue;
        //    playerPullController.AdjustPushPullSpeed(baseSpeed + Tier4Speed);
        //    playerPullController.GetComponent<PlayerLineController>().UpdateLineState(4);



        //}
        //else if(enemysKilledInCombo > 10)
        //{
        //    MultText.gameObject.SetActive(true);

        //    currentMult = 3;
        //    multColor = Color.green;
        //    playerPullController.AdjustPushPullSpeed(baseSpeed + Tier3Speed);
        //    playerPullController.GetComponent<PlayerLineController>().UpdateLineState(3);

        //}
        //else if(enemysKilledInCombo > 5)
        //{
        //    MultText.gameObject.SetActive(true);
        //    currentMult = 2;
        //    multColor = Color.yellow;
        //    playerPullController.AdjustPushPullSpeed(baseSpeed + Tier2Speed);
        //    playerPullController.GetComponent<PlayerLineController>().UpdateLineState(2);

        //}



        MultText.text = "x" + currentMult.ToString();
        MultText.color = multColor;

        scoreAdded = true;
        timeScoreAdded = Time.timeSinceLevelLoad;
        currentScore += (score * currentMult);
        CurrentScoreText.text = currentScore.ToString();
        AddedScoreText.gameObject.SetActive(true);
        AddedScoreText.text = "+" + (score * currentMult).ToString();


        if(currentScore > manager.ScoreToBeat)
        {
            manager.CompleteLevel();
        }

    }

    private void Update()
    {
        if (scoreAdded)
        {
            if(Time.timeSinceLevelLoad > TimeShowScore + timeScoreAdded)
            {
                scoreAdded = false;
                AddedScoreText.gameObject.SetActive(false);
            }
        }

        if(currentMult  > 1)
        {
            if(Time.timeSinceLevelLoad > timeScoreAdded + TimeHoldMult)
            {
                playerPullController.AdjustPushPullSpeed(baseSpeed);

                enemysKilledInCombo = 0;
                MultText.gameObject.SetActive(false);
                currentMult = 1;

            }
        }
    }
}

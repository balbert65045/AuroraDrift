using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ComboSystem : MonoBehaviour
{
    [SerializeField] TMP_Text TotalComboText;
    [SerializeField] TMP_Text CurrentComboText;
    [SerializeField] float TimeUntilComboDies = 2f;


    public int currentTotalCombo = 0;
    int currentCurrentCombo = 0;

    TimerClass comboTimer = new TimerClass(false);

    float initComboSize;
    float currentComboSize;

    float valueIncrease = .02f;
    float maxSize = 4f;

    public void ClearValues()
    {
        currentTotalCombo = 0;
        currentCurrentCombo = 0;
        CurrentComboText.enabled = false;
        TotalComboText.text = currentTotalCombo.ToString();

    }

    public void IncreaseCombo()
    {
        comboTimer = new TimerClass(true, TimeUntilComboDies, Time.time);
        CurrentComboText.enabled = true;

        UpdateCurrentCombo(1);
        UpdateTotalCombo(currentCurrentCombo);

        currentComboSize = initComboSize + currentCurrentCombo * valueIncrease;
        currentComboSize = Mathf.Min(currentComboSize, maxSize);
        CurrentComboText.transform.localScale = Vector3.one * currentComboSize;
        AdjustColor();
    }

    void AdjustColor()
    {
        float colorPercentage = 1 - Mathf.Clamp01(currentCurrentCombo / 30f);
        CurrentComboText.color = new Color(CurrentComboText.color.r, colorPercentage, CurrentComboText.color.b);
    }

    void UpdateCurrentCombo(int amount)
    {
        currentCurrentCombo += amount;
        CurrentComboText.text = currentCurrentCombo.ToString() + " HIT";
    }

    void UpdateTotalCombo(int amount)
    {
        currentTotalCombo += amount;
        TotalComboText.text = currentTotalCombo.ToString();
    }


    // Start is called before the first frame update
    void Start()
    {
        initComboSize = CurrentComboText.transform.localScale.x;
        currentComboSize = initComboSize;
    }

    public void ClearCombo()
    {
        currentCurrentCombo = 0;
        CurrentComboText.enabled = false;

        CurrentComboText.transform.localScale = Vector3.one * initComboSize;
        currentComboSize = initComboSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (comboTimer.IsOn())
        {
            if (comboTimer.TimerStillGoing(Time.time))
            {
                float percentage = comboTimer.percentageComplete(Time.time);
                if (percentage > .5f)
                {
                    CurrentComboText.transform.localScale = (currentComboSize - ((percentage - .5f) * currentComboSize/2)) * Vector3.one;
                }
            }
            else
            {
                ClearCombo();
            }
        }
    }
}

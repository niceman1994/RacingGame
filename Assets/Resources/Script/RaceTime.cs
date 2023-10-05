using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceTime : MonoBehaviour 
{
    [SerializeField] private Text Timecount;
    [SerializeField] private GameObject RaceEnd;
    [SerializeField] private Text RaceEndTime;

    float time;
    int minute;

    private void Start() 
    {
        time = 0.0f;
        minute = 0;
    }

    private void FixedUpdate() 
    {
        if (GameManager.Instance.CountNum == 0)
            PassingTime();
    }
    
    void PassingTime() 
    {
        if (Time.timeScale == 1) 
        {
            time += Time.fixedDeltaTime;

            if (GameManager.Instance.EndRace == true) 
            {
                RaceEnd.SetActive(true);
                RaceEndTime.text = Timecount.text;
                return;
            }
        }

        if (time > 60.0f) 
        {
            time -= 60.0f;
            minute += 1;
        }

        if (minute < 10) 
        {
            if (time < 10.0f)
                Timecount.text = "0" + minute.ToString() + " : 0" + time.ToString("F3");
            else
                Timecount.text = "0" + minute.ToString() + " : " + time.ToString("F3");
        }
        else 
        {
            if (time < 10.0f)
                Timecount.text = minute.ToString() + " : 0" + time.ToString("F3");
            else
                Timecount.text = minute.ToString() + " : " + time.ToString("F3");
        }
    }
}

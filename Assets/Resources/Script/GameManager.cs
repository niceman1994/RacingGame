using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : ManagerSingleton<GameManager> 
{
    [Header("GameObject")]
	public GameObject Car;
    public GameObject Logo;
    public GameObject Intro;

    [Header("Lap")]
    public Text Lapcount;
    public int Currentlap;
    public int Lastlap;
    public Transform UI;

    [Space]
    public float downForceValue;
    public bool StartRace;
    public bool EndRace;

    [Header("Countdown")]
    public Text countDown;
    public int countDownNum;

    private void Start() 
    {
        Application.targetFrameRate = 60;
        downForceValue = 500.0f;
        StartRace = false;
        EndRace = false;
        StartCoroutine(CountDown());
    }

    private void Update() 
    {
        RaceStart();
        Lapcount.text = Currentlap.ToString();

        if (Currentlap == Lastlap)
            EndRace = true;
    }

    void RaceStart() 
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            if (Logo.activeInHierarchy == true) 
            {
                Logo.SetActive(false);
                Intro.SetActive(true);
            }
            else 
            {
                Intro.SetActive(false);               
                UI.gameObject.SetActive(true);
                StartRace = true;
            }
        }
    }

    public IEnumerator CountDown() 
    {
        while (true) 
        {
            yield return null;

            if (StartRace == true) 
            {
                SoundManager.Instance.GameBGM[2].Play();
                countDown.text = countDownNum.ToString();
                yield return new WaitForSeconds(1.0f);

                countDownNum -= 1;
                countDown.text = countDownNum.ToString();
                yield return new WaitForSeconds(1.0f);

                countDownNum -= 1;
                countDown.text = countDownNum.ToString();
                yield return new WaitForSeconds(1.0f);

                countDownNum -= 1;
                countDown.text = "Start";
                yield return new WaitForSeconds(1.0f);
                countDown.gameObject.SetActive(false);

                if (countDownNum == 0) 
                {
                    countDownNum = 0;
                    break;
                }
            }
        }
    }

    public void LapcountUp()
    {
        if (Currentlap < Lastlap)
            Currentlap++;
    }
}

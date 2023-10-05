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
    public Text Count;
    public int CountNum;

    private void Start() 
    {
        Application.targetFrameRate = 60;
        downForceValue = 500.0f;
        StartRace = false;
        EndRace = false;
        CountNum = 3;
        StartCoroutine(countDown());
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

    public IEnumerator countDown() 
    {
        while (true) 
        {
            yield return null;

            if (StartRace == true) 
            {
                SoundManager.Instance.GameBGM[2].Play();
                Count.text = CountNum.ToString();
                yield return new WaitForSeconds(1.0f);

                CountNum -= 1;
                Count.text = CountNum.ToString();
                yield return new WaitForSeconds(1.0f);

                CountNum -= 1;
                Count.text = CountNum.ToString();
                yield return new WaitForSeconds(1.0f);

                CountNum -= 1;
                Count.text = "Start";
                yield return new WaitForSeconds(1.0f);
                Count.gameObject.SetActive(false);

                if (CountNum == 0) 
                {
                    CountNum = 0;
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

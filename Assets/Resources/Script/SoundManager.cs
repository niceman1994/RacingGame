using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundManager : ManagerSingleton<SoundManager> 
{
    public GameObject Pause;
    public Button Restartbutton;
    public AudioSource[] GameBGM;

    void Start() 
    {
        GameBGM[0].Play();
    }

    void Update() {
        if (GameManager.Instance.Logo.activeInHierarchy == false) 
        {
            GameBGM[0].Stop();

            if (!GameBGM[1].isPlaying)
                GameBGM[1].Play();
        }

        if (GameManager.Instance.Intro.activeInHierarchy == false && GameBGM[1].isPlaying)
            GameBGM[1].Stop();

        setPause();
    }

    public void ClickRestart() 
    {
        if (EventSystem.current.IsPointerOverGameObject())
            Pause.SetActive(true);
    }

    void setPause() {
        if (GameManager.Instance.countDownNum == 0) 
        {
            if (Input.GetKeyDown(KeyCode.Escape)) 
            {
                if (Pause.activeInHierarchy == false)
                {
                    Pause.SetActive(true);
                    GameBGM[3].Play();
                    Time.timeScale = 0;
                }
                else 
                {
                    Pause.SetActive(false);
                    GameBGM[3].Play();
                    Time.timeScale = 1;
                }
            }
        }
    }
}

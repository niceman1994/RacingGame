using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarBooster : MonoBehaviour
{
    [Header("Car Drift Info")]
    [SerializeField] private float carSpeed;
    [SerializeField] private float driftGauge;
    [SerializeField] private float driftTime;

    [Space(20)]
    [SerializeField] private TrailRenderer[] tireMarks;
    [SerializeField] private Image BoostGauge;
    [SerializeField] private GameObject[] boostItem;
    [SerializeField] private float boosterTime;
    [SerializeField] private bool usebooster;

    public ref float CarSpeed { get { return ref carSpeed; } }
    public TrailRenderer[] TireMarks { get { return tireMarks; } }
    public float BoosterTime { get { return boosterTime; } }
    public bool useBooster { get { return usebooster; } }

    void Start()
    {
        boostItem[0].SetActive(false);
        boostItem[1].SetActive(false);
        boosterTime = 3.5f;
        usebooster = false;
    }

    void FixedUpdate()
    {
        UseBooster();
        carSpeed = Mathf.Abs(transform.GetComponent<PlayerCar>().getCurrentSpeed());

        if (tireMarks[0].emitting == true && tireMarks[1].emitting == true)
        {
            driftGauge = driftTime * carSpeed * 0.003f;
            BoostGauge.fillAmount += driftGauge;
        }
        else if (tireMarks[0].emitting == false && tireMarks[1].emitting == false)
            driftGauge = 0.0f;
    }

    void Update()
    {
        GaugeUp();
    }

    public void TrailStartEmitter()
    {
        foreach (TrailRenderer trail in tireMarks)
        {
            driftTime += Time.deltaTime * 0.5f;
            trail.emitting = true;
        }
    }

    public void TrailStopEmitter()
    {
        foreach (TrailRenderer trail in tireMarks)
        {
            driftTime = 0.0f;
            trail.emitting = false;
        }
    }

    void GaugeUp()
    {
        if (BoostGauge.fillAmount == 1.0f && tireMarks[0].emitting == false &&
            tireMarks[1].emitting == false)
        {
            if (boostItem[0].activeInHierarchy == false)
            {
                boostItem[0].SetActive(true);
                BoostGauge.fillAmount = 0.0f;
            }
            else if (boostItem[0].activeInHierarchy == true &&
                boostItem[1].activeInHierarchy == false)
            {
                boostItem[1].SetActive(true);
                BoostGauge.fillAmount = 0.0f;
            }
            else if (boostItem[0].activeInHierarchy == true &&
                boostItem[1].activeInHierarchy == true)
                BoostGauge.fillAmount = 0.0f;
        }
    }

    void UseBooster()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && usebooster == false)
        {
            if (boostItem[0].activeInHierarchy == true &&
                boostItem[1].activeInHierarchy == false)
            {
                usebooster = true;
                boostItem[0].SetActive(false);
            }
            else if (boostItem[0].activeInHierarchy == true &&
                boostItem[1].activeInHierarchy == true)
            {
                usebooster = true;
                boostItem[1].SetActive(false);
            }
        }

        if (usebooster == true)
            boosterTime = boosterTime - Time.fixedDeltaTime >= 0.0f ? boosterTime - Time.fixedDeltaTime : 0.0f;

        if (boosterTime == 0.0f)
        {
            usebooster = false;
            boosterTime = 3.5f;
        }
    }
}

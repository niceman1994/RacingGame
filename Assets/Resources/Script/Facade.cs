using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facade : MonoBehaviour
{
    [SerializeField] private MainCamera mainCamera;
    [SerializeField] private Speedometer speedometer;
    [SerializeField] private CarBooster carBooster;
    [SerializeField] private CarSound carSound;

    private void FixedUpdate()
    {
        MainCameraMethod();
    }

    private void Update()
    {
        SpeedoMeterMethod();
        CarSoundMethod();
    }

    void MainCameraMethod()
    {
        mainCamera.MoveCamera(carBooster.useBooster);
    }

    void SpeedoMeterMethod()
    {
        speedometer.ShowArrow(ref carBooster.CarSpeed);
        speedometer.UpdateArrow(ref carBooster.CarSpeed);
    }

    void CarSoundMethod()
    {
        carSound.Driving(carBooster.TireMarks, ref carBooster.CarSpeed);
        carSound.DriveSound(ref carBooster.CarSpeed);
        carSound.Booster(carBooster.useBooster, carBooster.BoosterTime);
        carSound.PauseSound();
    }
}

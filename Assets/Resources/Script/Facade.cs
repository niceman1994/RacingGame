using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facade : MonoBehaviour
{
    [SerializeField] private MainCamera mainCamera;
    [SerializeField] private Speedometer speedometer;
    [SerializeField] private CarBooster carBooster;
    [SerializeField] private CarSound carSound;

    public void MainCameraMethod()
    {
        mainCamera.MoveCamera(carBooster.Booster);
    }

    public void SpeedoMeterMethod()
    {
        speedometer.ShowArrow(ref carBooster.CarSpeed);
        speedometer.UpdateArrow(ref carBooster.CarSpeed);
    }

    public void CarSoundMethod()
    {
        carSound.Driving(carBooster.TireMarks, ref carBooster.CarSpeed);
        carSound.DriveSound(ref carBooster.CarSpeed);
        carSound.Booster(carBooster.Booster, carBooster.BoosterTime);
        carSound.PauseSound();
    }
}

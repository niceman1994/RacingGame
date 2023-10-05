using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour 
{
    [Header("CarSounds")]
    [SerializeField] private AudioSource engineSound;
    [SerializeField] private AudioSource driftSound;
    [SerializeField] private AudioSource boosterSound;

    [Space]
    [SerializeField] private ParticleSystem[] Nitros;

    public void Driving(TrailRenderer[] trails, ref float _speed) 
    {
        if (Input.GetKey(KeyCode.UpArrow))
            DriveSound(ref _speed);
        else
            DriveSound(ref _speed);

        foreach (TrailRenderer element in trails)
        {
            if (element.emitting == true) {
                if (engineSound.isPlaying == true && driftSound.isPlaying == false) 
                {
                    engineSound.Stop();
                    driftSound.Play();
                }
            }
            else 
            {
                if (engineSound.isPlaying == false && driftSound.isPlaying == true) 
                {
                    driftSound.Stop();
                    engineSound.Play();
                }
                else if (engineSound.isPlaying == false && _speed > 0)
                    engineSound.Play();
                else if (engineSound.isPlaying == true && _speed == 0)
                    engineSound.Stop();
            }
        }
    }

    public void DriveSound(ref float _speed)
    {
        if (_speed <= 50.0f && _speed >= 0.0f)
            engineSound.volume = _speed * 0.01f;
    }

    public void Booster(bool _booster, float _boosterTime) 
    {
        foreach (ParticleSystem element in Nitros) 
        {
            if (_booster == true)
                element.Play();
            else
                element.Stop();
        }

        if (_boosterTime >= 1.0f && _booster == true) 
        {
            if (boosterSound.isPlaying == false)
                boosterSound.Play();
        }
    }

    public void PauseSound() 
    {
        if (Time.timeScale == 0) 
        {
            engineSound.Pause();
            driftSound.Pause();
            boosterSound.Pause();
        }
        else 
        {
            engineSound.UnPause();
            driftSound.UnPause();
            boosterSound.UnPause();
        }
    }
}

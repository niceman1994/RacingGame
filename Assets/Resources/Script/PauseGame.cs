using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private Camera Cam;

    private void Update() 
    {
        setPause();
    }

    void setPause() 
    {
        if (GameManager.Instance.StartRace == true)
        {
            Cam.depth = -2;
            Cam.gameObject.SetActive(false);
        }
    }
}

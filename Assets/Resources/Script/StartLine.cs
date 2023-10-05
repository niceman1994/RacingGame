using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartLine : MonoBehaviour
{
    private BoxCollider boxcollider;

    private void Awake() 
    {
        boxcollider = transform.GetComponent<BoxCollider>();
        boxcollider.enabled = false;
    }

    void Start() 
    {
        StartCoroutine(colliderEnable());
    }

    IEnumerator colliderEnable() 
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(10.0f);

        while (true) 
        {
            yield return null;

            if (GameManager.Instance.StartRace == true) 
            {
                yield return waitForSeconds;
                boxcollider.enabled = true;
                break;
            }
        }
    }
}

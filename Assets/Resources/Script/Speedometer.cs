using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour 
{
    [SerializeField] private RectTransform Arrow;
    [SerializeField] float MinArrowAngle = 0;
    [SerializeField] float MaxArrowAngle = -300f;
    [SerializeField] private Text speedText;

    void Start() 
    {
        speedText.text = "000";
    }
    
    void Update() 
    {
        //ShowArrow();
        //UpdateArrow();
    }

    public void ShowArrow(ref float _speed)
    {
        if (_speed < 10.0f)
            speedText.text = "00" + _speed.ToString();
        else if (_speed < 100.0f)
            speedText.text = "0" + _speed.ToString();
        else if (_speed >= 100.0f)
            speedText.text = _speed.ToString();
    }

    public void UpdateArrow(ref float _speed) 
    {
        var procent = _speed / MaxArrowAngle;
        var angle = (MaxArrowAngle - MinArrowAngle) * procent;

        if (Arrow.rotation.y >= -315.0f && Arrow.rotation.y <= 0.0f) 
        {
            Arrow.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);

            if (_speed > 315.0f)
                Arrow.rotation = Quaternion.AngleAxis(-315.0f, Vector3.forward);
        }
    }
}

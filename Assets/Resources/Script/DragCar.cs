using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragCar : MonoBehaviour, IDragHandler
{
    Animator ani;
    float rotateSpeed;

    void Start() 
    {
        ani = GetComponent<Animator>();
        rotateSpeed = 8.0f;
    }

    public void OnDrag(PointerEventData eventData) 
    {
        float x = eventData.delta.x * Time.deltaTime * rotateSpeed;

        transform.Rotate(0, -x, 0, Space.World);

        Debug.Log("ondrag");
    }

    void Update() 
    {
        if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            ani.enabled = false;
    }
}

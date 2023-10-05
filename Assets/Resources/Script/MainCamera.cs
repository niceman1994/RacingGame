using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour 
{
	[SerializeField] private GameObject cameraView;
	[SerializeField] private GameObject cameraPos;
	[SerializeField] private float speed;

    private void FixedUpdate() 
	{
		
	}

	public void MoveCamera(bool _booster)
    {
		if (_booster == false)
		{
			speed = 14.0f;

			gameObject.transform.position = Vector3.Lerp(transform.position,
			  cameraPos.transform.position,
			  Time.deltaTime * speed);
		}
		else
		{
			speed = 10.0f;

			gameObject.transform.position = Vector3.Lerp(transform.position,
			  cameraPos.transform.position,
			  Time.deltaTime * speed);
		}

		gameObject.transform.LookAt(cameraView.transform);
	}
}

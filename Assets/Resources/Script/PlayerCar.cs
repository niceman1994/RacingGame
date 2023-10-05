using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour 
{
	[Header("Tail Light")]
	[SerializeField] private GameObject LeftBackLight;
	[SerializeField] private GameObject RightBackLight;

	[Header("Wheel Collider")]
	[SerializeField] private WheelCollider colliderFR;
	[SerializeField] private WheelCollider colliderFL;
	[SerializeField] private WheelCollider colliderRR;
	[SerializeField] private WheelCollider colliderRL;

	[Header("Car Wheels")]
	[SerializeField] private Transform wheelTransformFL;
	[SerializeField] private Transform wheelTransformFR;
	[SerializeField] private Transform wheelTransformRL;
	[SerializeField] private Transform wheelTransformRR;

	[Header("Car Speed Info")]
	[SerializeField] private int maxTorque = 10;
	[SerializeField] private float currentSpeed;
	[SerializeField] private float decSpeed = 50.0f;
	[SerializeField] private float maxSpeed = 350.0f;
	[SerializeField] private float maxRevSpeed = 100.0f;

	[Space(20)]
	[SerializeField] private float SteerAngle = 45f;
	[SerializeField] private Facade carFacade;
	[SerializeField] private CarBooster carBooster;

	private float prevSteerAngle;
	private float power;

	Rigidbody rigid;
	bool drift;
	WheelFrictionCurve ForRRwheel;
	WheelFrictionCurve ForRLwheel;
	WheelFrictionCurve SideRLwheel;
	WheelFrictionCurve SideRRwheel;

	private void Awake() 
	{
		rigid = GetComponent<Rigidbody>();
	}

    void Start() 
	{
		rigid.centerOfMass = new Vector3(0.0f, -0.15f, 0.2f);
		power = 22.0f;
		drift = true;
		ForRRwheel = colliderRR.forwardFriction;
		ForRLwheel = colliderRL.forwardFriction;
		SideRRwheel = colliderRR.sidewaysFriction;
		SideRLwheel = colliderRL.sidewaysFriction;
	}

    void FixedUpdate() 
	{
		carFacade.MainCameraMethod();
		UpdateWheelPoses();
		Steer();

		if (GameManager.Instance.CountNum == 0) 
		{
			Control();
			Drift();
			Stiffness();
			BackLightOnOff();
		}

		if (GameManager.Instance.EndRace == true) 
		{
			colliderRR.brakeTorque = 45000.0f;
			colliderRL.brakeTorque = 45000.0f;
		}
	}

    void Update()
    {
		carFacade.SpeedoMeterMethod();
		carFacade.CarSoundMethod();
    }

    private void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag("Line")) 
		{
			GameManager.Instance.LapcountUp();

			if (GameManager.Instance.Currentlap == 2)
				SoundManager.Instance.GameBGM[4].Play();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Track"))
		{
			if (carBooster.TireMarks[0].emitting == true &&
				carBooster.TireMarks[1].emitting == true)
				DriftStop();
		}
	}

	void Steer() 
	{
		wheelTransformFL.Rotate(Vector3.up, (colliderFL.steerAngle - prevSteerAngle) * Time.fixedDeltaTime, Space.World);
		wheelTransformFR.Rotate(Vector3.up, (colliderFR.steerAngle - prevSteerAngle) * Time.fixedDeltaTime, Space.World);
	}

	void UpdateWheelPoses() 
	{
		UpdateWheelPos(ref colliderFL, ref wheelTransformFL);
		UpdateWheelPos(ref colliderFR, ref wheelTransformFR);
    }

	void UpdateWheelPos(ref WheelCollider _collider, ref Transform _tire) // 앞바퀴 위치 고정
	{
		Vector3 wheelPosition = Vector3.zero;
		Quaternion wheelRotation = Quaternion.identity;

		_collider.GetWorldPose(out wheelPosition, out wheelRotation);

		_tire.position = wheelPosition;
		_tire.rotation = wheelRotation;
	}

	void BackLightOnOff() 
	{
		if (currentSpeed == 0) 
		{
			LeftBackLight.SetActive(true);
			RightBackLight.SetActive(true);
		}
		else if (Input.GetKey(KeyCode.DownArrow)) 
		{
			if (currentSpeed < 0 && currentSpeed >= -maxSpeed) 
			{
				LeftBackLight.SetActive(true);
				RightBackLight.SetActive(true);
			}
			else if (currentSpeed > 0 && currentSpeed <= maxRevSpeed) 
			{
				LeftBackLight.SetActive(false);
				RightBackLight.SetActive(false);
			}
		}
		else if (Input.GetKey(KeyCode.UpArrow)) 
		{
			if (currentSpeed > 0 && currentSpeed <= maxRevSpeed) 
			{
				LeftBackLight.SetActive(true);
				RightBackLight.SetActive(true);
			}
			else if (currentSpeed < 0 && currentSpeed >= -maxSpeed) 
			{
				LeftBackLight.SetActive(false);
				RightBackLight.SetActive(false);
			}
		}
		else if (currentSpeed != 0) 
		{
			LeftBackLight.SetActive(false);
			RightBackLight.SetActive(false);
		}
	}

	void Control() 
	{
		rigid.AddForce(-transform.up * GameManager.Instance.downForceValue * rigid.velocity.magnitude);
		prevSteerAngle = colliderFR.steerAngle;

		// 차속(km/h) = 2π × 타이어반지름 × (엔진rpm)/(변속기 기어비 × 종감속 기어비) × 60/1000
		// 여기서는 임의로 변속하지 않기 때문에 변속기 기어비 × 종감속 기어비는 계산하지 않는다
		currentSpeed = 2 * 3.14f * colliderRL.radius * colliderRL.rpm * 60 / 1000;
		currentSpeed = Mathf.Round(currentSpeed);

		if (currentSpeed <= 0 && currentSpeed > -maxSpeed) 
		{
			if (carBooster.Booster == true)
				BoosterOn();
			else
				BoosterOff();
		}
		else if (currentSpeed >= 0 && currentSpeed < maxRevSpeed) 
		{
			colliderRR.motorTorque = -maxTorque * Input.GetAxis("Vertical") * power;
			colliderRL.motorTorque = -maxTorque * Input.GetAxis("Vertical") * power;
		}
		else 
		{
			colliderRR.motorTorque = 0;
			colliderRL.motorTorque = 0;
		}

		if (!Input.GetButton("Vertical")) 
		{
			colliderRR.brakeTorque = decSpeed;
			colliderRL.brakeTorque = decSpeed;
		}
		else 
		{
			colliderRR.brakeTorque = 0.0f;
			colliderRL.brakeTorque = 0.0f;
		}

		colliderFR.steerAngle = Input.GetAxis("Horizontal") * SteerAngle;
		colliderFL.steerAngle = Input.GetAxis("Horizontal") * SteerAngle;

		wheelTransformFL.Rotate(colliderFL.rpm / 30 * 360 * Time.fixedDeltaTime, 0.0f, 0.0f);
		wheelTransformFR.Rotate(colliderFR.rpm / 30 * 360 * Time.fixedDeltaTime, 0.0f, 0.0f);
		wheelTransformRL.Rotate(colliderRL.rpm / 30 * 360 * Time.fixedDeltaTime, 0.0f, 0.0f);
		wheelTransformRR.Rotate(colliderRR.rpm / 30 * 360 * Time.fixedDeltaTime, 0.0f, 0.0f);
	}

	void Drift()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			if (carBooster.TireMarks[0].emitting == false &&
			   carBooster.TireMarks[1].emitting == false)
			{
				if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
					DriftStart();
				else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
					DriftStart();
			}
			else if (carBooster.TireMarks[0].emitting == true &&
			   carBooster.TireMarks[1].emitting == true)
            {
				if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
					DriftStop();
				else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
					DriftStop();
			}
		}
	}

	void Stiffness()
	{
		if (carBooster.TireMarks[0].emitting == true &&
			carBooster.TireMarks[1].emitting == true && 
			!Input.GetKey(KeyCode.LeftShift))
		{
			SideRRwheel.stiffness += Time.deltaTime * 1.3f;
			SideRLwheel.stiffness += Time.deltaTime * 1.3f;

			if (SideRRwheel.stiffness >= 2.0f)
				DriftStop();
			else if (SideRRwheel.stiffness < 2.0f && SideRRwheel.stiffness >= 0.0f)
			{
				if (Input.GetKey(KeyCode.UpArrow))
				{
					if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
					{
						if (Input.GetAxis("Horizontal") > 0)
						{
							wheelTransformFL.Rotate(Vector3.up, (colliderFL.steerAngle - prevSteerAngle) * Time.fixedDeltaTime, Space.World);
							wheelTransformFR.Rotate(Vector3.up, (colliderFR.steerAngle - prevSteerAngle) * Time.fixedDeltaTime, Space.World);
							StiffnessUp();
						}
						else if (Input.GetAxis("Horizontal") < 0)
							StiffnessDown();
					}
					else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
					{
						if (Input.GetAxis("Horizontal") > 0)
							StiffnessDown();
						else if (Input.GetAxis("Horizontal") < 0)
						{
							wheelTransformFL.Rotate(Vector3.up, (colliderFL.steerAngle - prevSteerAngle) * Time.fixedDeltaTime, Space.World);
							wheelTransformFR.Rotate(Vector3.up, (colliderFR.steerAngle - prevSteerAngle) * Time.fixedDeltaTime, Space.World);
							StiffnessUp();
						}
					}
				}
				else if (Input.GetKey(KeyCode.DownArrow))
				{
					if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
					{
						if (Input.GetAxis("Horizontal") > 0)
						{
							wheelTransformFL.Rotate(Vector3.up, (colliderFL.steerAngle + prevSteerAngle) * Time.fixedDeltaTime, Space.World);
							wheelTransformFR.Rotate(Vector3.up, (colliderFR.steerAngle + prevSteerAngle) * Time.fixedDeltaTime, Space.World);
							StiffnessUp();
						}
						else if (Input.GetAxis("Horizontal") < 0)
							StiffnessDown();
					}
					else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
					{
						if (Input.GetAxis("Horizontal") > 0)
							StiffnessDown();
						else if (Input.GetAxis("Horizontal") < 0)
						{
							wheelTransformFL.Rotate(Vector3.up, (colliderFL.steerAngle + prevSteerAngle) * Time.fixedDeltaTime, Space.World);
							wheelTransformFR.Rotate(Vector3.up, (colliderFR.steerAngle + prevSteerAngle) * Time.fixedDeltaTime, Space.World);
							StiffnessUp();
						}
					}
				}
			}
		}
	}

	void DriftStart()
	{
		drift = true;
		carBooster.TrailStartEmitter();
		SideRRwheel.stiffness = 0.0f;
		SideRLwheel.stiffness = 0.0f;

		colliderRR.sidewaysFriction = SideRRwheel;
		colliderRL.sidewaysFriction = SideRLwheel;

		transform.Rotate(new Vector3(0.0f, Input.GetAxis("Horizontal") * SteerAngle * Time.fixedDeltaTime, 0.0f), Space.World);
	}

	void DriftStop()
	{
		drift = false;
		carBooster.TrailStopEmitter();
		SideRRwheel.stiffness = 2.0f;
		SideRLwheel.stiffness = 2.0f;

		colliderRR.sidewaysFriction = SideRRwheel;
		colliderRL.sidewaysFriction = SideRLwheel;
	}

	void StiffnessUp()
	{
		SideRRwheel.stiffness += Time.fixedDeltaTime * 1.0f;
		SideRLwheel.stiffness += Time.fixedDeltaTime * 1.0f;

		colliderRR.sidewaysFriction = SideRRwheel;
		colliderRL.sidewaysFriction = SideRLwheel;

		transform.Rotate(new Vector3(0.0f, Input.GetAxis("Horizontal") * (SteerAngle + 20.0f) * Time.fixedDeltaTime, 0.0f), Space.World);
	}

	void StiffnessDown()
	{
		SideRRwheel.stiffness -= Time.fixedDeltaTime * 0.8f;
		SideRLwheel.stiffness -= Time.fixedDeltaTime * 0.8f;

		colliderRR.sidewaysFriction = SideRRwheel;
		colliderRL.sidewaysFriction = SideRLwheel;

		transform.Rotate(new Vector3(0.0f, Input.GetAxis("Horizontal") * (SteerAngle + 20.0f) * Time.fixedDeltaTime, 0.0f), Space.World);
	}

	void BoosterOn()
	{
		ForRRwheel.stiffness = 1.5f;
		ForRLwheel.stiffness = 1.5f;

		colliderRR.forwardFriction = ForRRwheel;
		colliderRL.forwardFriction = ForRLwheel;

		colliderRR.motorTorque = -maxTorque * Input.GetAxis("Vertical") * (power + 10.0f);
		colliderRL.motorTorque = -maxTorque * Input.GetAxis("Vertical") * (power + 10.0f);
	}

	void BoosterOff()
	{
		ForRRwheel.stiffness = 2.0f;
		ForRLwheel.stiffness = 2.0f;

		colliderRR.forwardFriction = ForRRwheel;
		colliderRL.forwardFriction = ForRLwheel;

		colliderRR.motorTorque = -maxTorque * Input.GetAxis("Vertical") * power;
		colliderRL.motorTorque = -maxTorque * Input.GetAxis("Vertical") * power;
	}

	public ref float getCurrentSpeed() 
	{
        return ref currentSpeed;
    }
}

using UnityEngine;
using System.Collections;

public class HeroScript : MonoBehaviour {
	//Forward,backward
	public float HorizontalStepSpeed;
	public float VerticalStepSpeed;
	public bool IsForwardMoving;
	public bool IsForwardMovingInProcess;
	public float ForwardStepTimeDuration;
	public float StartTimeOfForwardStep;
	public float CurrentHorizontalSpeed;
	public float CurrentVerticalSpeed;
	public float CurrentVerticalStepSpeed;
	
	//Jump
	public float VerticalJumpSpeed;
	public bool IsJumpInProgress;
	public bool IsJumpStart;
	public float JumpDurationTime;
	public float StartJumpTime;
	public float FinishJumpTime;
	public bool OnGround;
	public float JumpTimeOut;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
	 IsForwardMoving = false;
	 IsJumpStart = false;
	 if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) && !IsForwardMovingInProcess)
		{
			IsForwardMoving = true;			
			CurrentHorizontalSpeed = Mathf.Abs(HorizontalStepSpeed);
			CurrentVerticalStepSpeed = Mathf.Abs(VerticalStepSpeed);
		}
	 if(Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow) && !IsForwardMovingInProcess)
		{
			CurrentHorizontalSpeed = -Mathf.Abs(HorizontalStepSpeed);
			CurrentVerticalStepSpeed = Mathf.Abs(VerticalStepSpeed);
			IsForwardMoving = true;
			
		}
			
	 if(Input.GetKey(KeyCode.W) || Input.GetKey (KeyCode.UpArrow) && OnGround && !IsJumpInProgress )
		{
			IsJumpStart = true;
			CurrentVerticalStepSpeed = 0;
			CurrentVerticalSpeed = VerticalJumpSpeed;
		}
		CheckForwardMoving();
		CheckJump();
	 	IsOnGround();
	}
		
	void CheckForwardMoving()
	{
		if(IsForwardMoving)
		{
			IsForwardMovingInProcess = true;
			StartTimeOfForwardStep = Time.time;
		}
	if(IsForwardMovingInProcess )
		{
			if(Time.time - StartTimeOfForwardStep<ForwardStepTimeDuration)
				{
					transform.localPosition+=new Vector3(CurrentHorizontalSpeed,CurrentVerticalStepSpeed,0)*Time.deltaTime;
				}
			else
			{
				transform.localPosition+=new Vector3(CurrentHorizontalSpeed,-CurrentVerticalStepSpeed,0)*Time.deltaTime;
			}
			if(Time.time - StartTimeOfForwardStep>2*ForwardStepTimeDuration)
			{
				IsForwardMovingInProcess = false;
			}
		}
	}
	void CheckJump()
	{
		if(IsJumpStart)
		{
			IsJumpInProgress = true;
			StartJumpTime = Time.time;
		}
		if(IsJumpInProgress)
		{
			if(Time.time - StartJumpTime <= JumpDurationTime)
			{
				transform.localPosition+=new Vector3(0,CurrentVerticalSpeed,0)*Time.deltaTime;
			}
			else
			{
				IsJumpInProgress = false;	
			}
		}
	}
	
	void IsOnGround()
	{
		if(!OnGround)
		{
			if(!IsJumpInProgress)
			{
				transform.localPosition+=new Vector3(0,-CurrentVerticalSpeed,0)*Time.deltaTime;
			}
		}
	}
	
	/*void OnTriggerStay(Collider other)
	{
		OnGround = other.gameObject.name == "floor";
	}*/
	void OnTriggerExit(Collider other)
	{
		OnGround = !(other.gameObject.tag == "Floor");
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Floor")
		{
			float platformPosition = other.gameObject.transform.localPosition.y+other.gameObject.transform.localScale.y/2;
			float heroPosition = transform.localPosition.y-transform.localScale.y/2;
			if(platformPosition<=heroPosition)
			{
				OnGround = true;
			}
			IsForwardMovingInProcess = false;
			IsJumpInProgress = false;
		}
	}
}

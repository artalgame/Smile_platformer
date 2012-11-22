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
	public float JumpTimeOut = 2;
	public float LastJumpTime = float.MinValue;
	
	public Material heroMaterial;
	public float LastChangeFrame = float.MinValue;
	public float TimeOutToChangeFrame;
	public float BackVerticalSpeed;
	
	public bool IsLookAtRight = true;
	public bool IsWallInRight = false;
	public bool IsWallInLeft = false;
	public bool IsDied;
	
	public int CountOfApples;
	// Use this for initialization
	void Start () {
		CountOfApples = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
	 IsForwardMoving = false;
	 IsJumpStart = false;
	 if((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !IsForwardMovingInProcess &&!IsWallInRight)
		{
			IsLookAtRight = true;
			IsForwardMoving = true;			
			CurrentHorizontalSpeed = Mathf.Abs(HorizontalStepSpeed);
			CurrentVerticalStepSpeed = Mathf.Abs(VerticalStepSpeed);
		}
	 if((Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) && !IsForwardMovingInProcess && !IsWallInLeft)
		{
			IsForwardMoving = true;
			IsLookAtRight = false;
			CurrentHorizontalSpeed = -Mathf.Abs(HorizontalStepSpeed);
			CurrentVerticalStepSpeed = Mathf.Abs(VerticalStepSpeed);	
		}
			
	 if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && OnGround && !IsJumpInProgress && Time.time-LastJumpTime>JumpTimeOut)
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
			if(Time.time - LastChangeFrame > TimeOutToChangeFrame)
			{
				LastChangeFrame = Time.time;
				var offset = heroMaterial.mainTextureOffset;
				heroMaterial.mainTextureOffset =new Vector2(offset.x+0.25f,offset.y);
			}
			if(Time.time - StartTimeOfForwardStep < ForwardStepTimeDuration)
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
			LastJumpTime = Time.time;
			BackVerticalSpeed = 0;
		}
		if(IsJumpInProgress)
		{
			if(Time.time - StartJumpTime <= JumpDurationTime)
			{
				transform.localPosition+=new Vector3(0,CurrentVerticalSpeed,0)
					*Time.deltaTime*((JumpDurationTime-(Time.time-StartJumpTime))/JumpDurationTime);
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
				BackVerticalSpeed+=(400f*Time.deltaTime);
				transform.localPosition+=new Vector3(0,-BackVerticalSpeed,0)*Time.deltaTime;
			}
		}
	}
	
	void OnTriggerStay(Collider other)
	{
	}
	void OnTriggerExit(Collider other)
	{
		if((other.gameObject.tag == "Floor"))
		{
			OnGround = false;
		}
		if(other.gameObject.tag == "Platform")
		{
		float platformPosition = other.gameObject.transform.localPosition.y+other.gameObject.transform.localScale.y/2;
			float heroPosition = transform.localPosition.y-transform.localScale.y/2;
			if(platformPosition<=heroPosition)
			{
				OnGround = false;
			}
		}
			else
		{
			var platformPosition = other.gameObject.transform.localPosition;
			var heroPosition = transform.localPosition;
				if(platformPosition.x>heroPosition.x)
				{
					IsWallInRight = false;
				}
					else
				{
					IsWallInLeft = false;
				}
		}
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Platform")
		{
			float platformPosition = other.gameObject.transform.localPosition.y+other.gameObject.transform.localScale.y/2;
			float heroPosition = transform.localPosition.y-transform.localScale.y/2;
			if(platformPosition<=heroPosition)
			{
				OnGround = true;
			}
			else
			{
				platformPosition = other.gameObject.transform.localPosition.x;
				heroPosition = transform.localPosition.x;
				if(platformPosition>heroPosition)
				{
					IsWallInRight = true;
				}
					else
				{
					IsWallInLeft = true;
				}
			}
			IsForwardMovingInProcess = false;
			IsJumpInProgress = false;
		}
		if(other.gameObject.tag == "Floor")
		{
			OnGround = true;
			IsForwardMovingInProcess = false;
			IsJumpInProgress = false;
		}
		if(other.tag == "Apple")
		{
			CountOfApples++;
			Destroy(other.gameObject);
		}
		if(other.tag == "Enemy")
		{
			IsDied = true;
		}
	}
}

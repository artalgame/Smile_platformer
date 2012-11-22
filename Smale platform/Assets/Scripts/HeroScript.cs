using UnityEngine;
using System.Collections;

public class HeroScript : MonoBehaviour {
	//Forward,backward
	public float HorizontalStepSpeed;
	public bool IsForwardMoving;
	public bool IsForwardMovingInProcess;
	public float ForwardStepTimeDuration;
	public float StartTimeOfForwardStep;
	public float CurrentHorizontalSpeed;
	public float CurrentVerticalSpeed;
	public float BackVerticalSpeed;//speed, return on ground
	
	//Jump
	public float VerticalJumpSpeed;
	public bool IsJumpInProgress;
	public bool IsJumpStart;
	public float JumpDurationTime;
	public float StartJumpTime;
	
	public float JumpTimeOut;
	public float LastJumpTime = float.MinValue;
	
	public Material heroMaterial;
	public float LastChangeFrame = float.MinValue;
	public float TimeOutToChangeFrame;
	
	
	public bool IsGoToRight = true;
	public bool IsWallAtRight = false;
	public bool IsWallAtLeft = false;
	public bool IsDied;
	
	public bool OnGround;//Hero is on ground
	
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
	 if((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !IsForwardMovingInProcess && !IsWallAtRight)
		{
			IsGoToRight = true;
			IsForwardMoving = true;			
			CurrentHorizontalSpeed = Mathf.Abs(HorizontalStepSpeed);
			var offset = heroMaterial.mainTextureOffset;
			heroMaterial.mainTextureOffset =new Vector2(offset.x,0.5f);
		}
	 if((Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) && !IsForwardMovingInProcess && !IsWallAtLeft)
		{
			IsForwardMoving = true;
			IsGoToRight = false;
			CurrentHorizontalSpeed = -Mathf.Abs(HorizontalStepSpeed);	
			var offset = heroMaterial.mainTextureOffset;
			heroMaterial.mainTextureOffset =new Vector2(offset.x,0);
		}
			
	 if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && OnGround && !IsJumpInProgress && Time.time-LastJumpTime>JumpTimeOut)
		{
			IsJumpStart = true;
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
					transform.localPosition+=new Vector3(CurrentHorizontalSpeed,0,0)*Time.fixedDeltaTime;
				}
			else
			{
				transform.localPosition+=new Vector3(CurrentHorizontalSpeed,0,0)*Time.fixedDeltaTime;
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
					*Time.fixedDeltaTime*((JumpDurationTime-(Time.time-StartJumpTime))/JumpDurationTime);
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
				BackVerticalSpeed+=(400f*Time.fixedDeltaTime);
				transform.localPosition+=new Vector3(0,-BackVerticalSpeed,0)*Time.fixedDeltaTime;
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
			IsWallAtLeft = false;	
			IsWallAtRight = false;
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
				heroPosition = transform.localPosition.x+transform.localScale.x/2*((BoxCollider)collider).size.x;
				if(platformPosition>heroPosition)
				{
					IsWallAtRight = true;
				}
					else
				{
					IsWallAtLeft = true;
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
		if(other.tag == "Enemy" || other.tag == "Abyss")
		{
			IsDied = true;
		}
	}
}

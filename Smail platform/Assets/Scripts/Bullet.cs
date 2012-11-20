using UnityEngine;

using System.Collections;

public class Bullet : MonoBehaviour {
	public static float STANDART_SPEED = 300f;
	public float LifeTime = 5f;
	public float StartTime;
	public Vector3 Speed;
	// Use this for initialization
	void Start () {
		StartTime = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(StartTime+LifeTime<=Time.time)
		{
			Destroy(this.gameObject);
			return;
		}
		transform.localPosition+=Speed*Time.deltaTime;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Floor")
		{
			Destroy(this.gameObject);
		}
	}
}

using UnityEngine;

using System.Collections;

public class Bullet : MonoBehaviour {
	public static float STANDART_SPEED = 300f;
	public float LifeTime = 5f;
	public float StartTime;
	public Vector3 Speed;
	public float Damage = 1f;
	// Use this for initialization
	void Start () {
		StartTime = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(StartTime+LifeTime<=Time.time)
		{
			DestroyBullet();
			return;
		}
		transform.localPosition+=Speed*Time.deltaTime;
	}
	
	public void DestroyBullet()
	{
		Destroy(this.gameObject);
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Floor")
		{
			DestroyBullet();
		}
	}
}

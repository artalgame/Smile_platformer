using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public float Lifes;
	public Vector3 Speed;
	public bool IsGoToRight;//Enemy direction
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(IsGoToRight)
		{
			transform.localPosition+=Speed*Time.fixedDeltaTime;
		}
		else
		{
			transform.localPosition-=Speed*Time.fixedDeltaTime;
		}
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Edge")
		{
			IsGoToRight=!IsGoToRight;
		}
		if(other.tag == "Bullet")
		{
			Bullet bulletParams= (Bullet)other.GetComponent<Bullet>();
			Lifes-=bulletParams.Damage;
			bulletParams.DestroyBullet();
			if(Lifes<=0)
			{
				DestroyEnemy();
			}
		}
	}
	public void DestroyEnemy()
	{
		Destroy(this.gameObject);
	}
}

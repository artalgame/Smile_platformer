using UnityEngine;
using System.Collections;

public class HeroShooting : MonoBehaviour {
	public Bullet BulletPrefab;
	public HeroScript HeroScript;
	public float LastShootTime = float.MinValue;
	public float ShootTimeOut;
	// Use this for initialization
	void Start () {
		HeroScript =(HeroScript)GameObject.FindGameObjectWithTag("Player")
			.GetComponent<HeroScript>();
	}
	
	// Update is called once per frame
	void Update () {
	if(Input.GetKey(KeyCode.Space) && (Time.time - LastShootTime>=ShootTimeOut))
		{
			CreateShoot();
		}
	}
	void CreateShoot()
	{
		LastShootTime = Time.time;
		GameObject newBullet = (GameObject)GameObject.Instantiate(
			BulletPrefab.gameObject,transform.localPosition,new Quaternion(0,0,0,0));
		Bullet newBulletSettings = (Bullet)newBullet.GetComponent<Bullet>();
		if(HeroScript.IsGoToRight)
		{
			newBulletSettings.Speed = new Vector3(Bullet.STANDART_SPEED,0,0);
		}
		else
		{
			newBulletSettings.Speed = new Vector3(-Bullet.STANDART_SPEED,0,0);
		}
	}
}

using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	
	public GameObject Apples;
	public bool IsVictory;
	public bool IsDefeat;
	public HeroScript Hero;
	// Use this for initialization
	void Start () {
		IsVictory = false;
		IsDefeat = false;
		Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		var freeApples = Apples.GetComponentsInChildren<Transform>();
		if( freeApples.Length== 1)
		{
			IsVictory = true;		
		}
		else
			if(Hero.IsDied)
			{
				IsDefeat = true;
			}
	}
	void OnGUI()
	{
		if(IsVictory)
		{
			DrawMenu("You win");
		}
		if(IsDefeat)
		{
			DrawMenu("Defeat");
		}
		if(Hero != null)
		{
			GUI.Label(new Rect(30,30,50,50),"Apples: "+Hero.CountOfApples.ToString());
		}
	}
    void DrawMenu(string menuTitle)
	{
		Time.timeScale = 0;
		GUI.Box(new Rect(300,200,200,120), menuTitle);
		if(GUI.Button(new Rect(325,225,150,25),"New game"))
		{
		    LoadGame();
		}
		if(GUI.Button(new Rect(325,260,150,25),"Quit"))
		{
			Application.Quit();
		}
	}
	void LoadGame()
	{
		 Application.LoadLevel(Application.loadedLevel);
	}
}

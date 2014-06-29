using UnityEngine;
using System.Collections;

public class MainMenu : Game 
{
	
	private static MainMenu instance;
	
	public static MainMenu Instance
	{
		get { return instance ?? (instance = new GameObject("MainMenu").AddComponent<MainMenu>()); }
		
	}

	public bool startPressed = false;
	private int startNumberAnts = 1;
	// Use this for initialization
	void Start () {
	
	}
	private void OnGUI(){
		if(!startPressed){
			if(GUI.Button(new Rect(20,40,120,20), "start")) {
				Debug.Log("Start Pressed");
				startPressed = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public int getAntNum(){
		return startNumberAnts;
	}
}

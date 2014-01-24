using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

public class Play : Game 
{

	private static Play instance;
	
	public static Play Instance
	{
		get { return instance ?? (instance = new GameObject("Play").AddComponent<Play>()); }
		
	}

	public GameObject Grassland;
	
	public override void startSetup()
	{
		Debug.Log("playSetup");

		/*
		 * //Place Prefab
		GameObject grassland = (GameObject)Instantiate(Resources.Load("Grassland"));
		Vector3 testPosition = new Vector3 (0, 0, 0);
		grassland.transform.position = testPosition;
		*/

	}

}

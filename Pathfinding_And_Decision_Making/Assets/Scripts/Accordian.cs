using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Accordian : Game 
{
	/*public static Accordian Instance {get; private set;} //This set works but method doesnt work
	
	void Awake()
	{
		Instance = this;	
	}*/
	private static Accordian instance;
	
	public static Accordian Instance
	{
		 get { return instance ?? (instance = new GameObject("Accordian").AddComponent<Accordian>()); }
    
	}
	public bool startPressed = false;
	GUIContent[] comboBoxList;
	ComboBox comboBoxControl;// = new ComboBox();
	GUIStyle listStyle = new GUIStyle();

	/*private static Accordian instance;
	private static Accordian Instance 
    {

        get 

            {

            if (instance == null) 

                {

               

                GameObject notificationObject = GameObject.FindGameObjectWithTag("Accordian");		
                 //instance = (Accordian) notificationObject.GetComponent(typeof(Accordian));
				instance = (Accordian)notificationObject.AddComponent(typeof(Accordian));
				instance = (Accordian) notificationObject.GetComponent(typeof(Accordian));
                }
			
            return instance;

		}

    }

	 public static Accordian GetInstance()

    {

        return Instance;

    }*/

	public override void Deal()
	{
		Debug.Log("Accordian: Dealing Now");
		ttest ();
		//SetBoardState.E
		//GameObject obj;
		//obj = (GameObject)Instantiate (PlayingCard, transform.position,transform.rotation);
		//Instantiate (PlayingCard, new Vector3(0,0,0), Quaternion.identity);
		//PlayingCard card = new PlayingCard();
	}

	
	private void ttest()
	{
		Debug.Log ("test");


						comboBoxList = new GUIContent[6];
						comboBoxList [0] = new GUIContent ("Open");
						comboBoxList [1] = new GUIContent ("Swamp");
						comboBoxList [2] = new GUIContent ("Grass");
						comboBoxList [3] = new GUIContent ("Obst.");
						comboBoxList [4] = new GUIContent ("Goal");
						comboBoxList [5] = new GUIContent ("Start");
		
						listStyle.normal.textColor = Color.white; 
						listStyle.onHover.background =
			listStyle.hover.background = new Texture2D (2, 2);
						listStyle.padding.left =
			listStyle.padding.right =
				listStyle.padding.top =
				listStyle.padding.bottom = 4;
		
						comboBoxControl = new ComboBox (new Rect (1050, 100, 50, 50), comboBoxList [0], comboBoxList, "button", "box", listStyle);
	


	}
	
	private void OnGUI () 
	{
		comboBoxControl.Show();

		if(GUI.Button(new Rect(20,40,80,20), "Start")) {
			//Application.LoadLevel(1);
			Debug.Log("Start Game Pressed");
			startPressed = true;
		}
	}
}
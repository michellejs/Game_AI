using UnityEngine;

public sealed class PlayGameState :  State<GameManager> {
	
	bool isPlaying = false;
	//string nextState = "";
	
	static readonly PlayGameState instance = new PlayGameState();
	public static PlayGameState Instance {
		get 
		{
			return instance;
		}
	}
	static PlayGameState() { }
	private PlayGameState() { }
	
	
	public override void Enter (GameManager g) {
		if (g.Location != Locations.setBoardState) {
			Debug.Log("GamePlaying");
			g.ChangeLocation(Locations.setBoardState);
			isPlaying = true;
			//Accordian.GetInstance().Deal ();
			Debug.Log ("setGame");
			Play.Instance.startSetup();
			//setBoard();
			
		}
	}
	public void setBoard()
	{
		//Debug.Log ("setBoard");
		//Accordian.GetInstance().Deal();
		//Game.Instance.startSetup();
	}
	//Execute is the same as update, its just being called from cardGameStateMachine
	public override void Execute (GameManager g) {
		//Debug.Log("Now Playing accordian");
		//if(!isPlaying)	
		//g.ChangeState(nextState);
		//if (Setup.Instance.startPressed)				
			SetNextState (g, "mainMenu");
		//Setup.Instance.startPressed = false;
	}
	
	public override void Exit(GameManager g) {
		Debug.Log("Exiting Setup");
	}
	/*public void setIsPlayingAccordian(bool _play)
	{
		isPlaying = false;
		if(_play)
			isPlaying = true;
	}*/
	public override void SetNextState(GameManager g, string state)//This needs to be removed in all states once I get the GUI working
	{
		g.ChangeState (state);
	}
}


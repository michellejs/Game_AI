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

	//static int gridLength = 16;
	//static int gridHeight = 16;
	//Tile[,] grid = new Tile[gridLength,gridHeight];
	
	public override void Enter (GameManager g) {
		if (g.Location != Locations.playGameState) {
			g.ChangeLocation(Locations.playGameState);
			Debug.Log("Play Game State");
			Play.Instance.createGameBoard();
			Play.Instance.winState = false;
			//MainMenu.Instance.startPressed = false;
			//Play.Instance.createTerrain();
			//Play.Instance.startAnts(MainMenu.Instance.getAntNum());
			Play.Instance.playDisplayed = true;
			//Play.Instance.createTiles();
			//Play.Instance.displayGrid();
			//Play.Instance.startPathFinding();
		}
	}
	//Execute is the same as unity3d Update, its just being called from GameStateMachine
	//GameStateMachine only calls the Execute method of the current state.
	//Execute checks to see if Play.finishedPressed = true and if so changes to mainMenu state
	public override void Execute (GameManager g) {
		if (Play.Instance.finishPressed)				
			SetNextState (g, "mainMenu");
		Play.Instance.finishPressed = false;
	}
	
	public override void Exit(GameManager g) {
		Play.Instance.playDisplayed = false;
		//Play.Instance.clearGrid ();
		Debug.Log("Exiting Setup");
	}
	/*public void setIsPlayingAccordian(bool _play)
	{
		isPlaying = false;
		if(_play)
			isPlaying = true;
	}*/
	public override void SetNextState(GameManager g, string state)
	{
		g.ChangeState (state);
	}
}


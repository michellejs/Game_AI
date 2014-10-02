using UnityEngine;

public class MainMenuState :  State<GameManager> {

	
	//Each state is a singleton. The game manager switches states so
	//only one is active at a time.
	static readonly MainMenuState instance = new MainMenuState();
	public static MainMenuState Instance {

		get 
		{
				return instance;
		}
	}
	static MainMenuState() { }
	public MainMenuState() { }
	string selectedGame;
		
	//Enter runs once each time this state becomes active
	public override void Enter (GameManager g) {
		if (g.Location != Locations.mainMenu) {
			g.ChangeLocation(Locations.mainMenu);
		}
	}
	
	//Execute overrides Unity3ds Update() method
	//It executes once per frame when this state is active
	//Checks to see if(startPressed) and if true changes to the next state
	public override void Execute(GameManager g)
	{
		if (MainMenu.Instance.startPressed)				
			SetNextState (g, "playGameState");
	}
	//Runs once before exiting this state
	//Used for any cleanup necessary when leaving state
	public override void Exit(GameManager g) {

	}
	//Caused the GameManager to switch to the next state
	public override void SetNextState(GameManager g, string state)
	{
		g.ChangeState (state);
	}
}


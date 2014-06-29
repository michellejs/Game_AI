using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Play : Game 
{

	private static Play instance;
	
	public static Play Instance
	{
		get { return instance ?? (instance = new GameObject("Play").AddComponent<Play>()); }

		
	}
	//public static int gridLength = 16;
	//public static int gridHeight = 16;

	
	AIPlayer computerPlayer;
	GameObject go;


	public Texture downArrow;
	GUIStyle downButton = new GUIStyle ();
	public bool finishPressed = false;
	public bool playDisplayed = false;
	public bool playerTurn = true;
	public bool winState;
	int columns = 7;//maybe set this from the menu in case of different board size;
	int rows = 6;
	int[,] gameBoard;
	void Start(){
		computerPlayer = new AIPlayer ();

	}
	public void createGameBoard()
	{
		GameObject boardBottom = (GameObject)Instantiate (Resources.Load ("Gameboard_BottomWall"));
		List<GameObject> sideWalls;
		float startPos = -4.20f;
		for (int i=0; i<columns+1; i++) {
			GameObject sideWall = (GameObject)Instantiate (Resources.Load ("Gameboard_SideWall"));
			Vector3 temp = new Vector3(startPos,1,0);	
			sideWall.transform.position = temp;

			startPos += 1.2f;
		}
		gameBoard = new int[rows, columns];
		for (int i=0; i<columns; i++) {
			for(int j=0; j<rows; j++){
				gameBoard[j,i] = -1;
			}
		}




	}

	private void OnGUI(){

				

		if(playDisplayed){
			if(GUI.Button(new Rect(20,40,120,20), "Finish")) {
				//Application.LoadLevel(1);

				Debug.Log("Done Pressed");
				finishPressed = true;
			}
		}
		//Display column buttons if it is human players turn and there is no winner yet
		if ((playerTurn) && (!winState)) {
			GUI.Label(new Rect(200, 650, 240, 40), "White to move!");
			int pos = 190;
			for(int i=0; i<columns; i++){
				if(GUI.Button (new Rect(pos, 40, 60, 60), downArrow)){
					//call method to drop white token to correct slot
					dropCurrentPiece (i);
				}
				pos += 85;
			}

			

		}
	}
	//Wait for 1 second then get the Computer move and call the dropCurrentPiece method. Finally set the player 
	//turn to true.
	IEnumerator computerTurn(){
		yield return new WaitForSeconds(1);
		int computerMove = computerPlayer.aiMove();
		dropCurrentPiece (computerMove);
		playerTurn = true;
	}
	private bool updateBoard(int column){

		bool columnFull = false;
		bool done = false;
		int row = - 1;
		int filled = playerTurn == true ? 0 : 1;
		for (int i=0; i<rows; i++) {
			if(gameBoard[i, column] == -1){
				gameBoard[i, column] = filled;
				row = i;
				done = true;
				break;
			}
		}
		//If the Column is full do not let the player drop a piece in this column
		if (!done) {
			columnFull = true;
			return false;
		}
		//return true so game piece will be played. Also determine end game status
		else {
			for(int i=0; i<rows; i++){
				for(int j=0; j<columns; j++){
					Debug.Log (gameBoard[i,j]);
				}
			}
			winState = computerPlayer.determineWinState(gameBoard, row, column, filled);
			return true;
		}

	}
	private void dropCurrentPiece(int column){
		GameObject token;

		//these next few only need to be calculated once per game, could be moved to seperate method
		float pos = -3.6f;
		float[] x = new float[columns];
		for (int i=0; i<columns; i++) {
			x[i] = pos;
			pos += 1.2f;
		}


		if (playerTurn) {
			if(updateBoard(column)){
			token = (GameObject)Instantiate (Resources.Load ("Gameboard_WhiteChip"));
			Vector3 temp = new Vector3(x[column],4,0);	
			token.transform.position = temp;
				playerTurn = false;
				StartCoroutine(computerTurn ());
				//updateBoard(column);
			}
			else{
				//playerTurn = false;
				//StartCoroutine(computerTurn ());
			}
		} 
		else {
			token = (GameObject)Instantiate (Resources.Load ("Gameboard_BlackChip"));
			updateBoard (column);
			Vector3 temp = new Vector3(x[column],4,0);	
			token.transform.position = temp;
		}
		//Vector3 temp = new Vector3(x[column],4,0);	
		//token.transform.position = temp;


	}


	/*
	public void clearGrid(){//figure out how to destroy grid tiles
		System.Array.Clear (grid, 0, grid.Length);
		foreach (GameObject tile in tiles) {
			Destroy (tile);
		}
	}*/
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

	GameObject imageRock, imagePaper, imageScissors, imageLizard, imageSpock, textEndGame;
	GameObject buttonRock, buttonPaper, buttonScissors, buttonLizard, buttonSpock, buttonPlay, buttonQuit, buttonReset, buttonStats;
	List<GameObject> actionButtons, menuButtons, aiImages;
	int aiSelection =0;
	//This is called when the script is first activated. This script is activated
	//at runtime because it has been attached to a GameObject which starts off in 
	//play (Main Camera). 
	void Start () {
		setupGUI ();
		setAIResponsesInactive ();
		setActionButtonsInactive ();
	}
	private int aiChoice(){
		//obiously this needs to be improved
		return (Random.Range (0, 5));
	}
	//takes ints representing the ai and player choices.
	//returns 0 if it is a draw, 1 if the player wins
	//and -1 if the ai wins.
	private int determineWinner(int player, int ai){
		if (player == ai) {
			return 0;
		} 
		else {
			switch (player) {
			case 0: 
				if((ai==1)||(ai==4)){
					return -1;
				}
				else{
					return 1;
				}
			case 1: 
				if((ai==2)||(ai==3)){
					return -1;
				}
				else{
					return 1;
				}	
			case 2:
				if((ai==0)||(ai==4)){
					return -1;
				}
				else{
					return 1;
				}
			case 3:
				if((ai==0)||(ai==2)){
					return -1;
				}
				else{
					return 1;
				}
			default:
				if((ai==1)||(ai==3)){
					return -1;
				}
				else{
					return 1;
				}
			}
		}
	}
	private void playerChoice(int choice){
		setActionButtonsInactive ();
		int winner = determineWinner (choice, aiSelection);
		showEndGame (winner, choice, aiSelection);


		//update stats
		setMenuButtonsActive();
	}
	private void clearEndGame(){
		//reset player and ai icon colours
		//remove icons
		foreach (GameObject image in aiImages) {
			image.GetComponent<Image>().color = Color.white;
			image.SetActive(false);
		}
		foreach (GameObject button in actionButtons) {
			button.SetActive(false);
		}
		textEndGame.SetActive (false);

	}
	private void showEndGame(int winner, int player, int ai){
		Color colorPlayer = Color.white;
		Color colorAI = Color.white;
		string endText = "Should never be read";

		switch(winner){
		case 0: 
			colorPlayer = Color.blue;
			colorAI = Color.blue;
			endText = "DRAW";
			break;

		case -1: 
			colorPlayer = Color.red;
			colorAI = Color.green;
			endText = "LOSE";
			break;
		
		case 1:
			colorPlayer = Color.green;
			colorAI = Color.red;
			endText = "WIN";
			break;
		}
		endGame (player, ai, colorPlayer, colorAI, endText);
	}

	private void endGame(int player, int ai, Color colorPlayer, Color colorAI, string text){
		aiImages[ai].SetActive(true);
		aiImages[ai].GetComponent<Image>().color = colorAI;
		actionButtons[player].SetActive(true);
		actionButtons[player].GetComponentInChildren<Image>().color = colorPlayer;
		textEndGame.SetActive(true);
		textEndGame.GetComponent<Text>().text = text;
		textEndGame.GetComponent<Text>().color = colorPlayer;
	}
	public void reset(){
		//reset statistic history
	}
	public void playGame(){
		clearEndGame ();
		setMenuButtonsInactive ();
		aiSelection = aiChoice ();
		setActionButtonsActive ();
	}
	//Quits the game.
    public void quitGame(){
		Application.Quit ();
	}
	//makes the Images representing the AIs choice Inactive.
	//This makes them all invisible. 
	private void setAIResponsesInactive(){
		foreach (GameObject image in aiImages)
			image.SetActive (false);
	}
	//sets rock, paper, scissors, lizard and spock buttons
	//to inactive and makes them invisible
	private void setActionButtonsInactive(){
		foreach (GameObject button in actionButtons)
			button.SetActive (false);
	}
	//sets play and quit buttons to inactive, makes invisible
	private void setMenuButtonsInactive(){
		foreach (GameObject button in menuButtons)
			button.SetActive (false);
	}
	//Sets play and quit buttons to active and makes them visible
	private void setMenuButtonsActive(){
		foreach (GameObject button in menuButtons)
			button.SetActive (true);
	}
	//Sets rock, paper, scissors, lizard and spock buttons
	//to Active and makes them visible
	private void setActionButtonsActive(){
		foreach (GameObject button in actionButtons) {
			button.SetActive (true);
			button.GetComponentInChildren<Image>().color = Color.white;
		}
	}
	//Sets all of the variables to align with their proper GameObjects(Images, Buttons,
	//Text etc). Adds GUI objects into Lists in order to manipulate them easier.
	private void setupGUI(){
		imageRock = GameObject.Find ("Image_Rock");
		imagePaper = GameObject.Find ("Image_Paper");
		imageScissors = GameObject.Find ("Image_Scissors");
		imageLizard = GameObject.Find ("Image_Lizard");
		imageSpock = GameObject.Find ("Image_Spock");
		buttonRock = GameObject.Find ("Button_Rock");
		buttonRock.GetComponent<Button>().onClick.AddListener(delegate{playerChoice(0);});
		buttonPaper = GameObject.Find ("Button_Paper");
		buttonPaper.GetComponent<Button>().onClick.AddListener(delegate{playerChoice(1);});
		buttonScissors = GameObject.Find ("Button_Scissors");
		buttonScissors.GetComponent<Button>().onClick.AddListener(delegate{playerChoice(2);});
		buttonLizard = GameObject.Find ("Button_Lizard");
		buttonLizard.GetComponent<Button>().onClick.AddListener(delegate{playerChoice(3);});
		buttonSpock = GameObject.Find ("Button_Spock");
		buttonSpock.GetComponent<Button>().onClick.AddListener(delegate{playerChoice(4);});
		buttonPlay = GameObject.Find ("Button_Play");
		buttonQuit = GameObject.Find ("Button_Quit");
		textEndGame = GameObject.Find ("Text_EndGameStatus");
		textEndGame.SetActive (false);

		aiImages = new List<GameObject> ();
		aiImages.Add (imageRock);
		aiImages.Add (imagePaper);
		aiImages.Add (imageScissors);
		aiImages.Add (imageLizard);
		aiImages.Add (imageSpock);

		actionButtons = new List<GameObject> ();
		actionButtons.Add (buttonRock);
		actionButtons.Add (buttonPaper);
		actionButtons.Add (buttonScissors);
		actionButtons.Add (buttonLizard);
		actionButtons.Add (buttonSpock);

		menuButtons = new List<GameObject> ();
		menuButtons.Add (buttonPlay);
		menuButtons.Add (buttonQuit);
	}
}

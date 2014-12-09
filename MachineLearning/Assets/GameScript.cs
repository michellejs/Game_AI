using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class GameScript : MonoBehaviour {
	/*
 * 
 * Todo 
 * Show Actual stats when stat button pressed
 * come up with some algorithm to use stats to make a prediction
 * Display Flavour text "Rock crushes Scissors" etc
 * Metric 3 Always selects 0 - Why is this????
 * 
 * 
 * 
 */
	GameObject textEndGame, textRock, textPaper, textScissors, textLizard, textSpock, textPlayerWins, textComputerWins, textDraws;
	GameObject imageRock, imagePaper, imageScissors, imageLizard, imageSpock;
	GameObject buttonRock, buttonPaper, buttonScissors, buttonLizard, buttonSpock, buttonPlay, buttonQuit, buttonReset, buttonStats;
	GameObject textRandom;
	List<GameObject> actionButtons, menuButtons, aiImages, statsText;
	List<int> selectionHistory;
	int[] currentStats, aiSelection, metricWins;
	int totalGames, totalNonDraw, aiWins, playerWins, draws, randomAIWins, metric1AIWins, metric2AIWins, metric3AIWins;
	bool statsOn;
	int numberOfOptions = 5; //Rock, Paper, Scissors, Lizard, Spock

	//This is called when the script is first activated. This script is activated
	//at runtime because it has been attached to a GameObject which starts off in 
	//play (Main Camera). 
	void Start () {
		aiSelection = new int[5];
		statsOn = false;
		selectionHistory = new List<int> ();
		setupGUI ();
		setAIResponsesInactive ();
		setActionButtonsInactive ();
		//maybe change where these are initialized, they will be updated in getStats
		//saved values if they exist. Possibly initialize them there.
		//-also some of these might not be necessary. Remove whatever I'm not using.
		totalGames = 0;
		totalNonDraw = 0;
		aiWins = 0;
		playerWins = 0;
		draws = 0;
		metricWins =  new int[5]; //holds the number of wins for each metric
		for (int i=0; i<5; i++) {
			metricWins [i] = 0;//initialize to 0
		}
		getStats ();
		//This is not right. it will never show anything because
		//I make a new list right above this. However Player Prefs are not saved in IDE
		Debug.Log ("History");
		for (int i=0; i<selectionHistory.Count(); i++) {
			Debug.Log (selectionHistory [i]);
		}
		//keeps track of which choice the player makes
		currentStats = new int[5];

	}
	private int[] otherOptionBeatenBySelection(int selection){
		int[] options = new int[2];
		switch (selection) {
			case 0: 
				options[0] = 2;
				options[1] = 3;
				return options;
			break;
		case 1: 
				options[0] = 0;
				options[1] = 4;
				return options;
			break;
		case 2:
				options[0] = 1;
				options[1] = 3;
				return options;
			break;

		case 3:
				options[0] = 1;
				options[1] = 4;
				return options;
			break;

		default:
				options[0] = 0;
				options[1] = 2;
				return options;
			break;
		}
	}
	private int[] optionsToBeatPlayerChoice(int potentialChoice){
		int[] winOptions = new int[2];
		switch (potentialChoice) {
			case 0:
			winOptions[0] = 1;//paper covers rock
			winOptions[1] = 4;//spocks vaporizes rock
				break;
		case 1:
			winOptions[0] = 2;//scissors cuts paper
			winOptions[1] = 3;//Lizard eats paper
			break;
		case 2:
			winOptions[0] = 0;//rock crushes scissors
			winOptions[1] = 4;//spock smashes scissors
			break;
		case 3:
			winOptions[0] = 2;//scissors decapitate lizard
			winOptions[1] = 0;//rock crushes lizard
			break;
		default:
			winOptions[0] = 1;//paper disproves spock
			winOptions[1] = 3;//lizard poisons spock
			break;
		}
		return winOptions;
	}
	//Determine the ai's choice for all of the different algorithms,
	private int[] aiChoice(){
		int[] choices = new int[5];
		//Determine the ai's choice for all of the different algorithms,
		//The first method is randomly choosing one
		//of the 5 options.
		choices [0] = Random.Range (0, 5);
		choices [1] = getMetric1 ();
		choices [2] = getMetric2 ();
		choices [3] = getMetric3 ();
		choices [4] = getMetric4 ();
		Debug.Log ("AIChoice: " + choices [0] + choices [1] + choices [2] + choices [3] + choices [4]);
		return choices;
	}
	//Metric1 determines how likely each choice is likely to win based
	//on all of the previous games and assignes each choice a percentage
	//based on that and then uses that to choose
	//Whatever the most commonly picked choice is it can be beaten by two
	//different choices. Choose the one whose second win condition is the highest
	private int getMetric1(){
		//find percentage chance of each option
		double[] percents = new double[numberOfOptions];
		for (int i=0; i<numberOfOptions; i++) {
			if(currentStats[i] !=0){
				percents[i] = totalGames/currentStats[i];
			}
			else{
				currentStats[i]=0;
			}
		}
		//Generate a percentage
		double maximum = 100;
		double minimum = 0;
		System.Random random = new System.Random();
		double percent = random.NextDouble() * (maximum - minimum) + minimum;
		//Determine which choice.
		int choice = 0;
		for (int i=0; i<numberOfOptions; i++) {
			if(percents[i] >= minimum && percents[i]< percent){
				choice = i;
				break;
			}
		}
		//Find out the two options which will beat choice
		int[] choices = optionsToBeatPlayerChoice(choice);
		//find out which of those choices the player is most likely to pick
		if (currentStats [choices [0]] > currentStats [choices [1]]) {
			return choices [0];
		} 
		else {
			return choices [1];
		}
	}
	//metric 2 finds the most common choice, then finds the two choices which beat
	//that and determines which of those is most likely to be victoryous and chooses 
	//that one
	private int getMetric2(){
		//find option player is most likely to choose:
		int maxValue = currentStats.Max();
		int maxIndex = currentStats.ToList().IndexOf(maxValue);
		//get the two options which will beat most common player choice
		int[] ops = optionsToBeatPlayerChoice (maxIndex);	
		//find out which of those choices the player is most likely to pick
		if (currentStats [ops [0]] > currentStats [ops [1]]) {
			return ops [0];
		} 
		else {
			return ops [1];
		}
	}
	//Metric3 predict based on previous choice
	//Go back through and see what was chosen previous game
	//and what they were statistically most likely to pick 
	//after that. For example when the player chooses A
	//60% of the time they choose C next. This could be updated
	//in the future to take into account the AI choices as well
	private int getMetric3(){
		//Get the previous choice
		int lastChoice = selectionHistory.LastOrDefault ();
		int[] selHis = new int[5];
		//go through the users selection history and find out 
		//how often they have picked each choice directly after
		//they chose lastChoice
		for (int i=0; i<selectionHistory.Count()-1; i++) {
			if(selectionHistory[i] == lastChoice){
				selHis[selectionHistory[i+1]]++;
			}
		}
		//choose the highest one
		int maxValue = selHis.Max();
		int maxIndex = selHis.ToList().IndexOf(maxValue);
		//find the two options that beat it
		int[] ops = optionsToBeatPlayerChoice (maxIndex);
		//find the two things those options beat
		int[] first = new int[2];
		int[] second = new int[2];

		first = otherOptionBeatenBySelection (ops [0]);
		second = otherOptionBeatenBySelection (ops [1]);


		//also sift out the one matching original
		int one, two;
		if (first [0] != maxIndex) {
			one = first [0];
		}
		else {
			one = first[1];
		}
		if (second [0] != maxIndex) {
			two = second [0];
		} 
		else {
			two = second[1];
		}
		//choose whichever of them the player more
		//frequently selects the turn after they have
		//selected lastChoice and return that.
		int final;
		if (selHis [one] > selHis [two])
			final = selHis [one];
		else
			final = selHis [two];
		Debug.Log ("Metric 3 " + final);
		return final;
	}
	//This takes the N gram and compares based on that. 
	private int getMetric4(){
		return 0;

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
		currentStats [choice]++;//update the relevant choice for the player
		selectionHistory.Add (choice);//add the players choice to the list of player choices
		setActionButtonsInactive ();
		//determine metric to use for this game
		int winner = -100;
		int currentMetric = getCurrentMetric ();
		//for each metric: determineWinner
		for (int i=0; i<aiSelection.Length; i++) {
			int w = determineWinner(choice, aiSelection[i]);
			//if it is a win update the correct metric
			if( w == -1){
				metricWins[i]++;
			}
			if(i == currentMetric){
				totalGames++;
				winner = w;
				if(winner == -1){
					aiWins++;
				}
				if(winner == 0){
					draws++;
				}
				if(winner == 1){
					playerWins++;
				}
			}
		}
		if(statsOn)//the stats must be active to be updated
			updateStatsText ();
		showEndGame (winner, choice, aiSelection[currentMetric]);
		Debug.Log ("Current Metric: " + currentMetric + " Metrics: Random-" + metricWins [0] + " M1-" + metricWins [1] + " M2-" + metricWins [2] + " M3-" + metricWins [3]);

		setMenuButtonsActive();
	}
	//Use whichever metric is currently generating the most
	//wins. Default to Random if they are all currently equal
	private int getCurrentMetric(){
		int selection = 0;
		int temp = metricWins[0];
		if (metricWins[1] > temp) {
			temp = metricWins[1];
			selection = 1;
		}
		if (metricWins[2] > temp) {
			temp = metricWins[2];
			selection = 2;
		}
		if (metricWins[3] > temp) {
			temp = metricWins[3];
			selection = 3;
		}
		if (metricWins [4] > temp) {
			temp = metricWins[1];
			selection = 4;
		}
		return selection;
	}
	//Clear all of the current images and action button
	private void clearEndGame(){
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
	//Clears selectionHistory List then saves it to PlayerPrefs.History
	public void reset(){
		selectionHistory.Clear ();
		saveStats ();
	}
	//Is called when "Play" is pressed in the game.
	//Remove the "Win/lose" text from visibility, 
	//Remove the "Play" and "Quit" buttons from visibility.
	//Choose the response for the AI (No cheating, the AI picks
	//based on its algorithms before the player ever picks).
	//Make the Selection buttons visibile.
	public void playGame(){
		Debug.Log ("Current Stats: " + currentStats [0] + currentStats [1] + currentStats [2] + currentStats [3] + currentStats [4]);
		clearEndGame ();
		setMenuButtonsInactive ();
		aiSelection = aiChoice ();
		setActionButtonsActive ();
	}
	//PlayerPrefsX is a script I got off the internet, it is designed 
	//to make saving and retrieving PlayerPrefs variables easier
	//(since PlayerPrefs only saves strings);
	//This gets the History variable from stored in PlayerPrefs. 
	//PlayerPrefsX converts it from a string into an array. 
	//It is changed to a List to make it easier to add new stats.
	private void getStats(){
		int [] numberArray = PlayerPrefsX.GetIntArray ("History");
		selectionHistory = numberArray.ToList ();
	}
	//Saves the stats into PlayerPrefs
	private void saveStats(){
		int[] anArray = selectionHistory.ToArray();
		PlayerPrefsX.SetIntArray ("History", anArray);
	}
	//Quits the game.
	//Saves the stats.
    public void quitGame(){
		saveStats ();
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
	//change the status of statsOn and call
	//method to toggle view on/off
	public void toggleStats(){

		if (statsOn) {
			statsOn = false;
			stats (false);
		}
		else {
			statsOn = true;
			stats (true);
		}
	}
	//Toggles the visibilty of the statistics info
	//based on user pressing the "Stats" button.
	public void stats(bool status){
		foreach (GameObject text in statsText) {
			text.SetActive(status);
		}

	}
	//updates the stats text displayed
	private void updateStatsText(){
		//determine win percentage, avoid divide by 0
		double randomText = 0;
		if(metricWins[0]!= 0){
			randomText = totalGames/metricWins[0];
		}
		double m1 = 0;
		if (metricWins [1] != 0) {
			m1 = totalGames/metricWins[1];
		}
		double m2 = 0;
		if (metricWins [2] != 0){
			m2 = totalGames/metricWins[2];
		}
		double m3 = 0;
		if (metricWins [3] != 0) {
			m3 = totalGames/metricWins[3];
		}
		double m4 = 0;
		if (metricWins [4] != 0) {
			m4 = totalGames/metricWins[4];
		}
		//update text
		Text txt;
		//GameObject textRandom = GameObject.Find ("Text_Random");
		txt = textRandom.GetComponent<Text> ();
		txt.text = "Number of Wins using Random Metric: " + metricWins [1];
		//GameObject textTest = GameObject.Find ("Text_Test");
		//txt = textTest.GetComponent<Text> ();
		//string s = "player choice: ";
		//txt.text = s;


	}
	//Sets all of the variables to align with their proper GameObjects(Images, Buttons,
	//Text etc). Adds GUI objects into Lists in order to manipulate them easier.
	//Initiates and keeps a reference to all GameObjects
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
		textEndGame = GameObject.Find ("Text_Endgame");
		textEndGame.SetActive (false);

		//Toggle stats to off
		statsText = GameObject.FindGameObjectsWithTag ("stats").ToList ();
		stats (false);

		//I added these by index because aiImages.add() for some reason
		//ended up with the AIs "spock" and "scissors" switched.
		aiImages = GameObject.FindGameObjectsWithTag ("aiImages").ToList ();
		aiImages[0] = imageRock;
		aiImages[1] = imagePaper;
		aiImages[2] = imageScissors;
		aiImages[3] = imageLizard;
		aiImages[4] = imageSpock;

		actionButtons = GameObject.FindGameObjectsWithTag ("actionButtons").ToList ();
		actionButtons = new List<GameObject> ();
		actionButtons.Add (buttonRock);
		actionButtons.Add (buttonPaper);
		actionButtons.Add (buttonScissors);
		actionButtons.Add (buttonLizard);
		actionButtons.Add (buttonSpock);

		menuButtons = new List<GameObject> ();
		menuButtons.Add (buttonPlay);
		menuButtons.Add (buttonQuit);

		textRandom = GameObject.Find ("Text_Random");
	}
}

using UnityEngine;
using System.Collections;

public class AIPlayer {
	

	public int aiMove(){//eventually this should take game parameters
		int move = Random.Range (0, 6);
		return move;

	}
	int rows = 6;//make these dynamic
	int columns = 7;
	//This is a long way to do this. I could probably find a shorter way later. This checks the whole row, instead of just
	//the four squares surrounding the area
	public bool determineWinState (int[,] gb, int row, int column, int player){
		bool matchFound = false;
		//check vertical 
		//ensure that row is at least 4(or later numberInARow)
		if (row >= 4) {
			for (int i=0; i<rows - 3; i++) {//maybe make 3 = numberInARow-1
				if ((gb [i, column] == player) && (gb [i + 1, column] == player) && (gb [i + 2, column] == player) && (gb [i + 3, column] == player)) {
					matchFound = true;
				}
			}
		}
		//check horizontal - Right now check entire row
		for (int i=0; i<columns - 3; i++) {
			if((gb[row,i] == player)&&(gb[row,i+1] == player)&&(gb[row,i+2]==player)&&(gb[row,i+3]==player)){
				matchFound = true;
			}
		}
		//check forward slash
		//ensure that the colu
		if ((column <= 4)&&(row<=2)) {
			//find bottom left

			//subtract whichever number is smaller from both row and column
			if((column > 0)&&(row >0)){
				if(column>row){
					column -= row;
					row -= row;
				}
				else{
					row -= column;
					column -= column;
				}
			}
			///This is not right
			/// blah blah blah
			if((gb[row,column]==player)&&(gb[row+1,column+1]==player)&&(gb[row+2,column+2]==player)&&(gb[row+3,column+3]==player)){

			}
		}

		//check back slash 
		Debug.Log (matchFound);
		return matchFound;
	}

}

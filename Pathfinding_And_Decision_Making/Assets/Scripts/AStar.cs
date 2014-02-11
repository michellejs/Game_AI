using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour {

	Tile[,] grid;
	List<Tile> openList = new List<Tile>();
	List<Tile> closedList = new List<Tile>();
	List<Tile> fullList = new List<Tile>();
	//Tile startTile;//I'm not sure if I need this
	Tile endTile;

	bool foundPath = false;
	bool openListIsEmpty = false;

	int gridLength;
	int gridHeight;


	private void searchClosedList(){//supposed to check to ensure finish tile is in closed list - currently checking in open
		//bool endTileFound = false;
	      foreach (Tile t in closedList){
						if (t.gPoint.x == endTile.gPoint.x && t.gPoint.y == endTile.gPoint.y) {
								foundPath = true;
								Debug.Log ("!!!!!!!");
						}
		}

	}
	public void aStarStart(){
		gridLength = Play.Instance.getGridLength ();
		gridHeight = Play.Instance.getGridHeight ();
		grid = Play.Instance.getTileGrid ();
		getStartTile();
		do {
			nextLayer ();

		} while(foundPath == false && openListIsEmpty == false);
		Debug.Log ("Time to show path!!");
	}
	private int searchFullList(int x, int y){
		for (int i=0; i<fullList.Count; i++) {
			if(x == fullList[i].gPoint.x && y == fullList[i].gPoint.y)
				return i;
		}
		Debug.Log ("Search full list: If this message comes up tile in closed list");
		return -1;//Should only be called if tile in closed list
	}
	private int searchOpenList(int x, int y){
		for (int i=0; i<openList.Count; i++) {
			if(x == openList[i].gPoint.x && y == openList[i].gPoint.y)
				return i;
		}
		return -1;
	}
	private void addToOpenList(Tile current, int test){
		Tile n = fullList[test];
		if(n.getGScore() != 1000)//if tile is traversable
		{
			int h = getHScore(n);
			n.setHScore(h);
			n.setFScore();
			n.setParentSquare(current);

			openList.Add (n);
			fullList.RemoveAt(test);
		}
	}
	private void nextLayer (){

		Debug.Log ("entering next Layer");
		openList.Sort(delegate(Tile t1, Tile t2) { return t1.fScore.CompareTo(t2.fScore); });

		Tile current = openList [0];

		closedList.Add (openList [0]);
		List<Tile> openListCopy = new List<Tile> ();//make this a loop instead
		foreach (Tile t in openList)
						openListCopy.Add (t);
		openList.RemoveAt(0);


		foreach (Tile t in openListCopy) {
			//Add to openlist all tiles which are adjacent to each tile currently on the openlist
			//For North tile
			if(t.gPoint.y != 0){//if there is a tile to the north check if that tile is in the open list
				int test = searchOpenList(t.gPoint.x, t.gPoint.y-1);
				if(test == -1){//if tile is not in openList get index of tile from full list
					test = searchFullList(t.gPoint.x, t.gPoint.y-1);
					if(test != -1){
						addToOpenList(current, test);
					}
				}
				else{

				}
			}
			//For NE tile
			if(t.gPoint.x < gridLength-1 && t.gPoint.y != 0){//if there is a tile to the NE check if that tile is in the open list
				int test = searchOpenList(t.gPoint.x + 1, t.gPoint.y - 1);
				if(test == -1){//if tile is not in openList get index of tile from full list
					test = searchFullList(t.gPoint.x + 1, t.gPoint.y-1);
					if(test != -1){
						addToOpenList(current, test);
					}
				}
			}

			//For East tile
			if(t.gPoint.x < gridLength-1){//if there is a tile to the East check if that tile is in the open list
				int test = searchOpenList(t.gPoint.x + 1, t.gPoint.y);
				if(test == -1){//if tile is not in openList get index of tile from full list
					test = searchFullList(t.gPoint.x + 1, t.gPoint.y);
					if(test != -1){
						addToOpenList(current, test);
					}
				}
			}
			//For SE tile
			if(t.gPoint.x < gridLength -1 && t.gPoint.y < gridHeight -1){//if there is a tile to the SE check if that tile is in the open list
				int test = searchOpenList(t.gPoint.x + 1, t.gPoint.y+  1);
				if(test == -1){//if tile is not in openList get index of tile from full list
					test = searchFullList(t.gPoint.x + 1, t.gPoint.y + 1);
					if(test != -1){
						addToOpenList(current, test);
					}
				}
			}

			//For South tile
			if(t.gPoint.y < gridHeight - 1){//if there is a tile to the south check if that tile is in the open list
				int test = searchOpenList(t.gPoint.x, t.gPoint.y + 1);
				if(test == -1){//if tile is not in openList get index of tile from full list
					test = searchFullList(t.gPoint.x, t.gPoint.y +1);
					if(test != -1){
						addToOpenList(current, test);
					}
				}
			}

			//For SW tile
			if(t.gPoint.x != 0 && t.gPoint.y < gridHeight - 1){//if there is a tile to the SW check if that tile is in the open list
				int test = searchOpenList(t.gPoint.x - 1, t.gPoint.y + 1);
				if(test == -1){//if tile is not in openList get index of tile from full list
					test = searchFullList(t.gPoint.x - 1, t.gPoint.y + 1);
					if(test != -1){
						addToOpenList(current, test);
					}
				}
			}


			//For West tile
			if(t.gPoint.x != 0){//if there is a tile to the north check if that tile is in the open list
				int test = searchOpenList(t.gPoint.x -1, t.gPoint.y);
				if(test == -1){//if tile is not in openList get index of tile from full list
					test = searchFullList(t.gPoint.x -1, t.gPoint.y);
					if(test != -1){
						addToOpenList(current, test);
					}
				}
			}
			//For NW  tile
			if(t.gPoint.x != 0 && t.gPoint.y != 0){//if there is a tile to the SE check if that tile is in the open list
				int test = searchOpenList(t.gPoint.x - 1, t.gPoint.y - 1);
				if(test == -1){//if tile is not in openList get index of tile from full list
					test = searchFullList(t.gPoint.x - 1 , t.gPoint.y - 1);
					if(test != -1){
						addToOpenList(current, test);
					}
				}
				else{
					/*
					 * If it is on the open list already, check to see if this path to that square is better, using G cost 
				as the measure. A lower G cost means that this is a better path. If so, change the parent of the square 
				to the current square, and recalculate the G and F scores of the square. If you are keeping your open list 
				sorted by F score, you may need to resort the list to account for the change.
					 */
				}
			}


		}
		if (openList.Count == 0){//I really need to make sure this one is working correctly
			openListIsEmpty = true;
		}
		searchClosedList ();
	}
	private void getStartTile(){
		for (int i=0; i<grid.GetLength(0); i++) {
			for(int j=0; j<grid.GetLength(1); j++){
				int gs = grid[i,j].getGScore();//Get the GScore of each tile
				if(gs == 10000)//If gScore == 10000 tile is start square
				{
					openList.Add (grid[i,j]);//Add start tile to openList
				}
				else if(gs == -1)
				{
					endTile = grid[i,j];//Set the end tile
					fullList.Add (grid[i,j]);
				}
				else
				{
					fullList.Add (grid[i,j]);
				}
			}
		}
		//set the H and F scores for the start tile
		int h = getHScore (openList [0]);
		openList [0].setHScore(h);
		openList [0].setFScore ();

	}
	//H score is calculated by taking the number of grid tiles the start tile is away from the end tile horizontally and
	//adding it to the number of grid tiles the start tile is away from the end tile vertically.
	private int getHScore(Tile t){
		int h1;
		int h2;
		if (t.gPoint.x > endTile.gPoint.x) {
			h1 = t.gPoint.x - endTile.gPoint.x;
		}
		else {
			h1 = endTile.gPoint.x - t.gPoint.x;
		}
		if (t.gPoint.y > endTile.gPoint.y) {
			h2 = t.gPoint.y - endTile.gPoint.y;
		}
		else {
			h2 = endTile.gPoint.y - t.gPoint.y;
		}
		return h1 + h2;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

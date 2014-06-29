using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour {

	Tile[,] grid;
	List<Tile> openList = new List<Tile>();
	List<Tile> closedList = new List<Tile>();
	List<Tile> fullList = new List<Tile>();
	Tile startTile;
	Tile endTile;

	bool foundPath = false;
	bool openListIsEmpty = false;

	int gridLength;
	int gridHeight;


	private void searchClosedList(){
				foreach (Tile t in closedList) {
						if (t.gPoint.x == endTile.gPoint.x && t.gPoint.y == endTile.gPoint.y) {
								foundPath = true;
								showPath (t);
								
						}
				}

		}
	public void showPath(Tile current){
		Stack<Vector3> screenPoint = new Stack<Vector3> ();
		Stack<Vector2> gridPoint = new Stack<Vector2> ();
		Tile parent;
		bool routeComplete = false;
		while (!routeComplete) {
			parent = current.parentSquare;
			Vector3 sp = new Vector3(current.sPoint.x,current.sPoint.y, -6);
			screenPoint.Push(sp);
			Vector2 gp = new Vector2(current.gPoint.x, current.gPoint.y);
			gridPoint.Push (gp);
			//Tile temp = current;

			if(current == startTile){
				routeComplete = true;
				break;
			}
			current = current.parentSquare;
		}
		foreach (Vector2 v in gridPoint) {
			Debug.Log (v);
				}
		Debug.Log (gridPoint);

		//eventually put this in seperate method
		Vector2 start = screenPoint.Pop ();
		GameObject ant = (GameObject)Instantiate (Resources.Load ("Prefabs/Ant"));
		ant.transform.position = new Vector3 (start.x, start.y, -6);


		while (screenPoint.Count > 0) {
			//Debug.Log ("pop");

			Vector3 nextPos = screenPoint.Pop ();
			ant.transform.up = nextPos - ant.transform.position;
			//ant.transform.LookAt(nextPos);
			Vector3 temp = ant.transform.position;
			//ant.transform.position = Vector3.Lerp(temp, nextPos, 1f);
			StartCoroutine(pause());
			StartCoroutine(moveAnt (ant, temp, nextPos));

		}
		//Vector3 nextPos = screenPoint.Pop ();
		//ant.transform.up = nextPos - ant.transform.position;

	}
	IEnumerator moveAnt(GameObject ant, Vector3 start, Vector3 end){
		//Debug.Log ("moveAnt");
		//yield return new WaitForSeconds(1);
		//function MoveObject (thisTransform : Transform, startPos : Vector3, endPos : Vector3, time : float) {
	
		float i = 0.0f;
		float time = 10.0f;
			float rate = 1.0f/time;
			while (i < 1.0f) {
						i += Time.deltaTime * rate;
						ant.transform.position = Vector3.Lerp (start, end, i);

				}
		yield return 0;
	}
	IEnumerator pause(){
		yield return new WaitForSeconds (1);
	}

	public void aStarStart(){
		gridLength = Play.Instance.getGridLength ();
		gridHeight = Play.Instance.getGridHeight ();
		grid = Play.Instance.getTileGrid ();

		getStartTile();

		//Debug.Log ("StartTile " + startTile.gPoint.x + "  " + startTile.gPoint.y);
		do {
			//Debug.Log ("OpenList for nextLayer, g pos then parent gpos");
			foreach(Tile t in openList)
			{

				if(t.gPoint.x != startTile.gPoint.x || t.gPoint.y != startTile.gPoint.y){
					Debug.Log ("square " + t.gPoint.x + "   " + t.gPoint.y);
				//Tile p = t.parentSquare;
				Debug.Log ("parent " + t.parentSquare.gPoint.x + "  " + t.parentSquare.gPoint.y);
				}
				else
					Debug.Log ("square " + t.gPoint.x + "   " + t.gPoint.y);
			}
			nextLayer ();

		} while(foundPath == false && openListIsEmpty == false);
		//Debug.Log ("Time to show path!!");
	}
		

	private int searchFullList(int x, int y){
		for (int i=0; i<fullList.Count; i++) {
			if(x == fullList[i].gPoint.x && y == fullList[i].gPoint.y)
				return i;
		}
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
		//Debug.Log ("Add to OpenList");
		//Debug.Log ("Current(parent): " + current.gPoint.x + "  " + current.gPoint.y);

		Tile n = fullList[test];
		//Debug.Log ("Square being added" + n.gPoint.x + "  " + n.gPoint.y);
		if(n.getGScore() != 1000)//if tile is traversable
		{
			int h = getHScore(n);
			n.setHScore(h);
			n.setFScore();
			n.setParentSquare(current);

			openList.Add (n);
			//Debug.Log ("Adding to open: " + n.gPoint.x + "  " +n.gPoint.y);
			Tile p = n.parentSquare;
			//Debug.Log ("Parent Square of new " + p.gPoint.x + " " + p.gPoint.y);
			Tile t = fullList[test];
			//Debug.Log ("removing from full: " + t.gPoint.x + "  " + t.gPoint.y);
			fullList.RemoveAt(test);
		}
		//Debug.Log ("Check square added pos: " + n.gPoint.x + "  " + n.gPoint + " Parent " + n.parentSquare.gPoint.x + " " + n.parentSquare.gPoint.y)
		//Debug.Log ("open list again");
		foreach (Tile t in openList) {
			//Debug.Log (t.gPoint.x + " " + t.gPoint.y + " parent " + t.parentSquare.gPoint.x + " " + t.parentSquare.gPoint.y);
				}
	}
	private void nextLayer (){

		Debug.Log ("entering next Layer");
		//This sorts the open list by f-score with the lowest f-score being first


		openList.Sort(delegate(Tile t1, Tile t2) { return t1.fScore.CompareTo(t2.fScore); });

		//Make the first tile of the open list be the tile we start from
		Tile t = openList [0];
		//Debug.Log ("Current Tile in Next Layer " + current.gPoint.x + " " + current.gPoint.y);
		//Remove the first tile from the open list
		closedList.Add (openList [0]);
		//make a copy of the openList called openListCopy. The copy is so that we can remove each tile from the list
		//as it is evaluated in order to keep track of what has been checked without messing up the openList
		List<Tile> openListCopy = new List<Tile> ();
		foreach (Tile j in openList)
						openListCopy.Add (t);
		openList.RemoveAt(0);//remove current from open list


		//foreach (Tile t in openListCopy) {
			//Add to openlist all tiles which are adjacent to each tile currently on the openlist
			//For North tile
			if(t.gPoint.y != 0){//if there is a tile to the north check if that tile is in the open list
				int test = searchOpenList(t.gPoint.x, t.gPoint.y-1);
				if(test == -1){//if tile is not in openList get index of tile from full list
					test = searchFullList(t.gPoint.x, t.gPoint.y-1);
					if(test != -1){
						addToOpenList(t, test);
					}
				}

			}
			//For NE tile
			if(t.gPoint.x < gridLength-1 && t.gPoint.y != 0){//if there is a tile to the NE check if that tile is in the open list
				int test = searchOpenList(t.gPoint.x + 1, t.gPoint.y - 1);
				if(test == -1){//if tile is not in openList get index of tile from full list
					test = searchFullList(t.gPoint.x + 1, t.gPoint.y-1);
					if(test != -1){
						addToOpenList(t, test);
					}
				}
			}

			//For East tile
			if(t.gPoint.x < gridLength-1){//if there is a tile to the East check if that tile is in the open list
				int test = searchOpenList(t.gPoint.x + 1, t.gPoint.y);
				if(test == -1){//if tile is not in openList get index of tile from full list
					test = searchFullList(t.gPoint.x + 1, t.gPoint.y);
					if(test != -1){
						addToOpenList(t, test);
					}
				}
			}
			//For SE tile
			if(t.gPoint.x < gridLength -1 && t.gPoint.y < gridHeight -1){//if there is a tile to the SE check if that tile is in the open list
				int test = searchOpenList(t.gPoint.x + 1, t.gPoint.y+  1);
				if(test == -1){//if tile is not in openList get index of tile from full list
					test = searchFullList(t.gPoint.x + 1, t.gPoint.y + 1);
					if(test != -1){
						addToOpenList(t, test);
					}
				}
			}

			//For South tile
			if(t.gPoint.y < gridHeight - 1){//if there is a tile to the south check if that tile is in the open list
				int test = searchOpenList(t.gPoint.x, t.gPoint.y + 1);
				if(test == -1){//if tile is not in openList get index of tile from full list
					test = searchFullList(t.gPoint.x, t.gPoint.y +1);
					if(test != -1){
						addToOpenList(t, test);
					}
				}
			}

			//For SW tile
			if(t.gPoint.x != 0 && t.gPoint.y < gridHeight - 1){//if there is a tile to the SW check if that tile is in the open list
				int test = searchOpenList(t.gPoint.x - 1, t.gPoint.y + 1);
				if(test == -1){//if tile is not in openList get index of tile from full list
					test = searchFullList(t.gPoint.x - 1, t.gPoint.y + 1);
					if(test != -1){
						addToOpenList(t, test);
					}
				}
			}


			//For West tile
			if(t.gPoint.x != 0){//if there is a tile to the north check if that tile is in the open list
				int test = searchOpenList(t.gPoint.x -1, t.gPoint.y);
				if(test == -1){//if tile is not in openList get index of tile from full list
					test = searchFullList(t.gPoint.x -1, t.gPoint.y);
					if(test != -1){
						addToOpenList(t, test);
					}
				}
			}
			//For NW  tile
			if(t.gPoint.x != 0 && t.gPoint.y != 0){//if there is a tile to the SE check if that tile is in the open list
				int test = searchOpenList(t.gPoint.x - 1, t.gPoint.y - 1);
				if(test == -1){//if tile is not in openList get index of tile from full list
					test = searchFullList(t.gPoint.x - 1 , t.gPoint.y - 1);
					if(test != -1){
						addToOpenList(t, test);
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


		//}
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
					startTile = grid[i,j];
				}
				else if(gs == -1)
				{
					endTile = grid[i,j];//Set the end tile
					fullList.Add (grid[i,j]);
				}
				else
				{
					if(grid[i,j].getGScore() != 1000)
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

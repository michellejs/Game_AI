using UnityEngine;
using System.Collections;

public class AntState {

	protected AntStateMachine ant;

	public AntState (AntStateMachine ant) {
		this.ant = ant;
	} 

	virtual public void OnEnter () {}
	virtual public void OnExit () {}

	//virtual public void Jump () {}
	//virtual public void Throw () {}

	//virtual public void Left () {}
	//virtual public void Right () {}
	//virtual public void Idle () {}

	//virtual public void HitFloor () {}
	//virtual public void HitWall () {}
	//virtual public void HitPlayer (Collision2D coll) {}

	virtual public void Update () {}

}

using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour {




	private float turretRotationSpeed = 5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.S) && gameObject.tag == "T1")
			counterClockwise();
		if(Input.GetKey(KeyCode.J) && gameObject.tag == "T2")
			counterClockwise();
		if (Input.GetKey (KeyCode.F) && gameObject.tag == "T1")
			clockwise ();
		if (Input.GetKey (KeyCode.L) && gameObject.tag == "T2")
			clockwise ();
	}
	
	void counterClockwise()
	{   
		transform.parent.Rotate (Vector3.forward, turretRotationSpeed);
	}
	void clockwise()
	{   
		transform.parent.Rotate (Vector3.back, turretRotationSpeed);
	}
}

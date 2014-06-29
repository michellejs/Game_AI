using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	/// Object speed
	//public Vector2 speed = new Vector2(10, 19);

	/// Moving direction
	//public Vector2 direction = new Vector2(1, 1);//obviously I need to fix this and facing
	
	//private Vector2 movement;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 20f);//destroy bullet after 20 seconds
	}
	void bulletDirection(Vector2 d){
		//direction = d;
		}
	// Update is called once per frame
	void Update () {
		// 2 - Movement
		//movement = new Vector2(
		//	speed.x * direction.x,
		//	speed.y * direction.y);
	}
	void FixedUpdate()
	{
		// Apply movement to the rigidbody
		//rigidbody2D.velocity = movement;
		//Rigidbody2D.AddForce (Vector2 10,10);










		                     
	}
}



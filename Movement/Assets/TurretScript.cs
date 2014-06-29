using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour {

	public Rigidbody2D Bullet;
	public float bulletSpeed = 10f;
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
		if (Input.GetKeyDown (KeyCode.Space)) {
						fire ();
		}
	}
	void fire(){
		//if (true)// if (canAttack)
		//{
		//obj = currentObjContainer.transform.GetChild(0);
			//Rigidbody2D newBullet = (Rigidbody2D) Instantiate(Bullet, transform.position, transform.rotation);
		Rigidbody2D newBullet = (Rigidbody2D) Instantiate(Bullet, transform.GetChild(0).position, transform.rotation);
		//float xDir = transform.parent.rotation.x;
		//float yDir = transform.parent.eulerAngles.y;
			//Debug.Log("ydir  " + yDir);
		//Debug.Log (xDir + "    " + yDir  + "   " +  transform.rotation.y);
		//Debug.Log (transform.parent.tag);
		//Quaternion parent = transform.parent.rotation;
		//Debug.Log (parent);

		//Vector3 pivotRotation = parent * Vector3.forward;
		//Quaternion rotation = Quaternion.Euler(xAngle, yAngle, zAngle);
		//Vector3 direction = rotation * Vector3.forward;
		//Vector2 force = new Vector2 (xDir * bulletSpeed, yDir * bulletSpeed);
		//Vector3 pivotRotation = parent.ToEulerAngles ();
		//Debug.Log (pivotRotation);
		//transform.parent.eulerAngles = Vector3(180,0,0);
		//Debug.Log("parent " + transform.parent.localEulerAngles);
		//Debug.Log ("object " + transform.localEulerAngles);
		//Debug.Log ("child " + transform.GetChild (0).localEulerAngles);
		Vector2 force = new Vector2 (-100,-100);
		Debug.Log (force);
		newBullet.AddForce(force);
		//newBullet.velocity = transform.forward * bulletSpeed;
		//newBullet.transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
			//shootCooldown = shootingRate;
			
			// Create a new shot
			//var bulletTransform = Instantiate(bullet) as Transform;
			
			// Assign position
			//bulletTransform.position = transform.position;
			

			//This is where I need to send the bullet in the correct direction
			// The is enemy property
			//ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
			//if (shot != null)
			//{
			//	shot.isEnemyShot = isEnemy;
			//}
			
			// Make the weapon shot always towards it
			//MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
			//if (move != null)
			//{
		//		move.direction = this.transform.right; // towards in 2D space is the right of the sprite
		//	}
		//}
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

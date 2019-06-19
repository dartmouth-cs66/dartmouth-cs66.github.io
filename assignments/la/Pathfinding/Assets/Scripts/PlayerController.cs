using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public int speed;
	public GameController gameController;
	public Camera mainCam;

	private Animator anim;

	// Update is called once per frame
//	void Update ()
//	{
//		float horizontal = Input.GetAxis ("Horizontal") * Time.deltaTime * speed;
//		float vertical = Input.GetAxis ("Vertical") * Time.deltaTime * speed;
//		gameObject.transform.Translate (new Vector3 (horizontal, 0, vertical));
//	}

	void Start ()
	{
		anim = gameObject.GetComponent<Animator> ();
		Debug.Log ("On start: " + mainCam);
	}

	void Update ()
	{
		// WASD Directional Input
		if (Input.GetKey(KeyCode.W))
		{
			MoveToDegree(0);
		}
		else if (Input.GetKey((KeyCode.A)))
		{
			MoveToDegree(90);
		}
		else if (Input.GetKey((KeyCode.S)))
		{
			MoveToDegree(180);
		}
		else if (Input.GetKey((KeyCode.D)))
		{
			MoveToDegree(270);
		}
		else
		{
			// if there is no WASD input, then the player is not moving
			anim.SetBool("playerMoving", false);
		}
	}

	private void MoveToDegree(int degree)
	{
		// get the current camera rotation along the y axis
		float camRot = mainCam.transform.eulerAngles.y;

		// detach the camera and rotate the player
		mainCam.transform.SetParent(null);
		transform.rotation = Quaternion.Euler(0, camRot - degree, 0);
		mainCam.transform.SetParent(transform);

		// move the player
		gameObject.transform.Translate(new Vector3(0, 0, 0.1f * speed * Time.deltaTime));

		// mark the Animator for a state transition, if applicable
		anim.SetBool("playerMoving", true);
	}


	// Triggered when the player visits a waypoint
	void OnTriggerEnter(Collider other)
	{
		WaypointController waypoint = other.GetComponent<WaypointController> ();
		if (other.CompareTag("Waypoint")) {
			VisitWaypoint (waypoint);
		}
	}
		
	/// <summary>
	/// Let the waypoint and the game controller know the player visited this waypoint
	/// The waypoint can update how it looks and the controller 
	/// can update score, end the game, etc.
	/// </summary>
	/// <param name="waypoint">Waypoint.</param>
	void VisitWaypoint(WaypointController waypoint)
	{
		waypoint.SendMessage("MarkVisited");
		gameController.SendMessage ("VisitWaypoint", waypoint.num);
	}

	/// <summary>
	/// To receive message telling the player they have been caught.
	/// Update animation appropriately and disable player movement.
	/// </summary>
	void PlayerCaught()
	{
		anim.SetBool ("playerCaught", true);
		speed = 0;  // disable player movement
	}
}

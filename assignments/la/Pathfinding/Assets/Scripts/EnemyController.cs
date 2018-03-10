using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class EnemyController : MonoBehaviour {

	public float minDistance;  /* min distance to the player for the enemy to start chasing the player */
	public GameController gameController;
//	public Transform player; /* what to chase when it gets too close to this enemy. Presumably, this is the player */
	public PlayerController player;

	private Transform[] waypointArray; /* waypoints to patrol */
	private int currentWaypointIndex;  /* index of the current waypoint the agent is going towards */
	NavMeshAgent agent; /* For setting the destination */


	private Animator anim;
	private bool playerIsCaught;
	private bool done;

	public enum State {
		Patrol,
		Chase,
		CatchPlayer
	}
	private State currentState;

	void Awake()
	{
		waypointArray = gameController.objectives;
	}

	void Start()
	{
		agent = this.GetComponent<NavMeshAgent> ();
		//		agent.autoBraking = false;
		anim = gameObject.GetComponent<Animator> ();
		currentState = State.Patrol;

		StartCoroutine (FSM ());
	}

	IEnumerator FSM()
	{
		while (!done) {
			Debug.Log ("Call to FSM()");
			yield return StartCoroutine (currentState.ToString ());
		}
	}

	IEnumerator Chase()
	{
		float dist = Vector3.Distance (player.transform.position, transform.position);
		while (dist < minDistance) {
			dist = Vector3.Distance (player.transform.position, transform.position); // recalculate distance on each function re-entrance
			if (playerIsCaught) {
				break;
			} else {
				TrackPlayer ();
			}
			yield return new WaitForSeconds (0.1f);
		}
		if (playerIsCaught)
			currentState = State.CatchPlayer;
		else
			currentState = State.Patrol;
	}

	/// <summary>
	/// Go between waypoints to prevent the player from getting to them
	/// </summary>
	IEnumerator Patrol()
	{
		float dist = Vector3.Distance (player.transform.position, transform.position);
		while (dist > minDistance) {
			dist = Vector3.Distance (player.transform.position, transform.position);
			if (!agent.pathPending && agent.remainingDistance < 0.5f) {
				GoToNextPoint ();
			}
			yield return new WaitForSeconds(0.1f);
		}
		currentState = State.Chase;
	}

	/// <summary>
	/// Update any rendering (the cat, for instance), wait a bit, then let
	/// the game controller know of this event.
	/// </summary>
	IEnumerator CatchPlayer()
	{
		RenderCaught ();
		yield return new WaitForSeconds (2f);
		Debug.Log ("Call to caught");
		gameController.SendMessage ("PlayerCaught");
		done = true;
		yield return null;
	}

	// -- UTILITY FUNCTIONS --
	// 		Do not handle any of the state transitions here; simply do functional
	//		things, such as setting destinations and so on

	void RenderCaught()
	{
		anim.SetBool ("playerCaught", true);
		agent.isStopped = true;
	}

	void GoToNextPoint()
	{
		if (waypointArray.Length == 0) {
			Debug.LogWarning ("No points to go to");
			return;
		}
		// Set the agent to go to the currently selected point
		agent.destination = waypointArray[currentWaypointIndex].position;

		// Choose the next point (cycle around the end of the array)
		currentWaypointIndex = (currentWaypointIndex + 1) % waypointArray.Length;
	}

	private void TrackPlayer()
	{
		Debug.Log ("Call to TrackPlayer");
		if (player != null) {
			Vector3 targetVector = player.transform.position;
			agent.SetDestination (targetVector);
			anim.SetBool ("playerCaught", false);
		} else {
			Debug.LogError ("No player desination defined in AgentMove.cs");
		}
	}

	/// <summary>
	/// When the enemy collides with (catches) the player, this is triggered.
	/// Set a boolean parameter checked on every iteration of the Chase state
	/// and act accordingly.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("On Trigger. Tag: " + other.tag);
		// TODO question. Should I change the state here directly?
		if (other.CompareTag("Player")) {
			playerIsCaught = true;
//			other.gameObject.SendMessage ("PlayerCaught");
			player.SendMessage ("PlayerCaught");
			Debug.Log ("TYPE" + other.gameObject.GetType());
		}
	}
}

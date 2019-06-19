using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public Transform[] objectives;
	public Transform player;
	public Transform[] arr;
	public GameObject restartPanel;

	private bool gameIsOver = false;
	private HashSet<int> visitedWaypoints;
	private int score = 0;
	private float startTime;

	void Awake()
	{
		restartPanel.SetActive (false);
	}

	// Use this for initialization
	void Start ()
	{
		visitedWaypoints = new HashSet<int>();
		startTime = Time.time;
	}

	private void EndGame(bool playerWon)
	{
		Text messageTextObj = restartPanel.GetComponentsInChildren<Text> () [1];
		if (playerWon) {
			score = (int)(100 / (Time.time - startTime));
			messageTextObj.text = string.Format("You win! Your score is {0:D}", score);
		} else {
			messageTextObj.text = "You lose :(";
		}
		restartPanel.SetActive (true);
	}


	public void ReloadScene()
	{
		SceneManager.LoadScene (0);
	}

	/// <summary>
	/// Keep track of which waypoints have been visited to know
	/// when to end the game
	/// </summary>
	/// <param name="waypointId">Waypoint identifier.</param>
	void VisitWaypoint(int waypointId)
	{
		visitedWaypoints.Add (waypointId);
		Debug.LogFormat ("{0} waypoints visited", visitedWaypoints.Count);
		if (objectives.Length == visitedWaypoints.Count) {
			EndGame (true);
		}
	}

	/// <summary>
	/// Message for the other game objects to trigger
	/// </summary>
	void PlayerCaught()
	{
		gameIsOver = true;
		Debug.Log ("PlayerCaught called");
		EndGame (false);
	}
}

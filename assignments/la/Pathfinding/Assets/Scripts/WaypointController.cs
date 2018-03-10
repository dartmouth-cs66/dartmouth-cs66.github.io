using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour {
	public int num;
	public Material defaultMaterial;
	public Material visitedMaterial;

	/// <summary>
	/// Update the look of the waypoint
	/// </summary>
	void MarkVisited()
	{
		GetComponent<Renderer> ().material = visitedMaterial;
	}
}

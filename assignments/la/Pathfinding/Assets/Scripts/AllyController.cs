using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AllyController : MonoBehaviour {

	[SerializeField]
	Transform _destination;
	public float minDistance;

	NavMeshAgent _navMeshAgent;


	// Use this for initialization
	void Start ()
	{
		_navMeshAgent = this.GetComponent<NavMeshAgent> ();

		if (_navMeshAgent == null) {
			Debug.LogError ("Nav mesh agent component not attached to " + gameObject.name);
		} else {
			SetDestination ();
		}
	}

	// Update is called once per frame
	void Update ()
	{
		SetDestination ();
	}
		
	/// <summary>
	/// Sets the destination for this character.
	/// Call this only if the agent is within a certain distance of the
	/// player
	/// </summary>
	private void SetDestination()
	{
		if (_destination != null) {
			Vector3 targetVector = _destination.transform.position;
			_navMeshAgent.SetDestination (targetVector);
		}			
	}
}

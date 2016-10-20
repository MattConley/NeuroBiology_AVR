using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

	public enum State {
		Open,
		Closed
	}

	// Properties

	[SerializeField] private GameObject gate;
	[SerializeField] private float rotation = 45.0f;
	[SerializeField] private State state;


	// Methods

	public void Open() {
		if (state != State.Open)
			gate.transform.Rotate(0, 0, rotation);
	}

	public void Close() {
		if (state != State.Closed)
			gate.transform.Rotate(0, 0, -rotation);
	}
		
}

using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	public GameObject emptygraph;
	public GameObject CubeObject;

	private ChangeColor otherScript;
	private double [,] newMatTwo;

	// Use this for initialization
	void Start () {
		otherScript = CubeObject.GetComponent<ChangeColor>();
		newMatTwo = otherScript.newMat;

		Debug.Log(newMatTwo[1,1]);
		
	}
	
	
}

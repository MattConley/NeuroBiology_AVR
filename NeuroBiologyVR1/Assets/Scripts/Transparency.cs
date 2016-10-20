using UnityEngine;
using System.Collections;

public class Transparency : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var color = gameObject.GetComponent<Renderer>().material.color;
        var newColor = new Color(color.r, color.g, color.b, 0.5f);
        gameObject.GetComponent<Renderer>().material.color = newColor;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

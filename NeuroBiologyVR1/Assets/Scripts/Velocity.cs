using UnityEngine;
using System.Collections;

public class Velocity : MonoBehaviour {
    public int x;
    public int y;
    public int z;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(x,y,z);
	}
}

using UnityEngine;
using System.Collections;

public class TransformGraph : MonoBehaviour {

	//public ParticleSystem graph;

	private float scale = 1;
	private float increment = 12;
	private float difference = 0;


public void SetTransformSystem(float value){

		if(value < scale){
			difference = Mathf.Abs(value - scale);
			scale = value;
			this.gameObject.GetComponent<BoxCollider>().center = this.gameObject.GetComponent<BoxCollider>().center - new Vector3(0, difference*increment, 0);
			print(this.gameObject.GetComponent<BoxCollider>().center);
			//graph.transform.position = new Vector3(graph.transform.position.x,graph.transform.position.y+difference*increment,graph.transform.position.z);
		}

		else if(value == scale){
			scale = value;
		}
		else{
			difference = Mathf.Abs(value - scale);
			scale = value;
			this.gameObject.GetComponent<BoxCollider>().center = this.gameObject.GetComponent<BoxCollider>().center + new Vector3(0, difference*increment, 0);
			//graph.transform.position = new Vector3(graph.transform.position.x,graph.transform.position.y-difference*increment,graph.transform.position.z);
			print(this.gameObject.GetComponent<BoxCollider>().center);
		}
	}
}
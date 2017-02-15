using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class TransformDown : MonoBehaviour {

private float scale = 1;
private float increment = 5;
private float difference = 0;

void Start(){
	transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z);
}

public void SetTransformDown(float value){

		if(value < scale){
			difference = Mathf.Abs(value - scale);
			scale = value;
			transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y+difference*increment,transform.localPosition.z);
		}

		else if(value == scale){
			scale = value;
		}
		else{
			difference = Mathf.Abs(value - scale);
			scale = value;
			transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y-difference*increment,transform.localPosition.z);
		}
	}
}
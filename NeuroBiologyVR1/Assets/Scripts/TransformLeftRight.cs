using UnityEngine;
using System.Collections;

public class TransformLeftRight : MonoBehaviour {

private float scale = 5f;
private float increment = 5f;
private float difference = 0;

void Start(){
	transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z);
}

public void SetTransformLeft(float value){
	
		if(value < scale){
			difference = Mathf.Abs(value - scale);
			scale = value;
			transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z+ difference*increment);
		}

		else if(value == scale){
			scale = value;
		}
		else{
			difference = Mathf.Abs(value - scale);
			scale = value;
			transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z-difference*increment);
		}
	}
}
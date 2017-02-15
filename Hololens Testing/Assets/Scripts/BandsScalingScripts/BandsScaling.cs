using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class BandsScaling : MonoBehaviour {

private float scale = 1;

void Start()
{
		transform.localScale = new Vector3(scale, scale, 1);		
}

public void SetBandScale(float value){

		scale = value;
		transform.localScale = new Vector3(scale, scale, 1);
		//transform.localPosition = new Vector3(1, 1, 1);
		Start();		
	}
}

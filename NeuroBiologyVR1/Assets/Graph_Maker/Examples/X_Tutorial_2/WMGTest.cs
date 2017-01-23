using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WMGTest : MonoBehaviour {

	public GameObject emptyGraphPrefab;
	public GameObject CubeObject;
	public WMG_Axis_Graph graph;
	public WMG_Series series1;
	public List<Vector2> series1Data;

	private ChangeColor otherScript;
	private double [,] newMatTwo;
	private int x=20;
	private int t=20;
	public float recordingSite;
	

	void Start () {
		GameObject graphGO = GameObject.Instantiate(emptyGraphPrefab);
		graphGO.transform.SetParent(this.transform, false);
		graph = graphGO.GetComponent<WMG_Axis_Graph>();
		series1 = graph.addSeries();
		//graph.xAxis.AxisMaxValue = x;

	}
	

	public void RecieveSliderValue(float valuez){
		recordingSite = valuez;

	}

	// Use this for initialization
 void Update () {
		//otherScript = CubeObject.GetComponent<ChangeColor>();
		
		//VTwo = otherScript.V;
		//
		//Refresh();
		otherScript = CubeObject.GetComponent<ChangeColor>();
		
		newMatTwo = otherScript.newMat;
		List<Vector2> data = new List<Vector2>();

		for(int i=0; i<t-1; i++){

			data.Add(new Vector2((float)i, (float) newMatTwo[i,21]));
			
			series1.pointValues.SetList(data);
		
		}

		for(int i=0; i<t-1; i++){

			data.Add(new Vector2((float)i, (float) newMatTwo[i,30]));
			
			series1.pointValues.SetList(data);
		
		}
		//print(recordingSite);
	}

}

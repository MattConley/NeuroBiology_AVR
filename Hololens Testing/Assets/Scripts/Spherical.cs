using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;



public class Spherical : MonoBehaviour {
	/*
    public Renderer band;
    public float rValue;
    public float gValue;
    public float bValue;
    */
	public float multiplier;
	double maxVal = .001;
	float delta = .0001f;
	ArrayList x = new ArrayList();
	ArrayList v0 = new ArrayList();
	ArrayList t;

	//public List<GameObject> Bands;
	public List<Renderer> FinalBands;

	//Cell Paremeters

	float Ri = 36f;                 //Ohms/cm
	float Cm = 1 * Mathf.Pow(10, -6);         //F/cm^2
	float Rm = 1 / (36 * Mathf.Pow(10, -3));  //cm^2/S (Ohms*cm^2)
	double[,] newMat;
	//Matrix 1:11
	//101:11

	void Start()
	{
		//Setting Up Array
		//Bands = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Bands"));
		//foreach (GameObject band in Bands) {
		//FinalBands.Add (band.GetComponent<Renderer> ());
		//}
		float diameter = 2 * Mathf.Pow(10, -6) * multiplier;   //Diameter
		float ri = 4f * Ri / (Mathf.PI * Mathf.Pow (diameter, 2f));
		float cm = Cm * Mathf.PI * diameter;
		float rm = Rm / (Mathf.PI * diameter);

		float lamda = Mathf.Sqrt (rm / ri); // Space Constant
		float tau = rm * cm; //Time Constant
		//Voltage over space
		/*float dx = 100f * Mathf.Pow (10, -6);

		for (float i = 0; i < maxVal; i += delta) {
			x.Add (i);
			v0.Add (2.0 * Mathf.Exp (-1f * i / lamda));
		}

		t = new ArrayList ();
		delta = .000001f;
		maxVal = .0001;
		for (float i = 0; i < maxVal; i += delta) {
			t.Add (i);
		}

		double[,] V = new double[t.Count, x.Count];

		for (int i = 0; i < x.Count; i++) {
			for (int row = 0; row < t.Count; row++) {
				V [row, i] = (double)v0 [i] * Mathf.Exp (-1.0f * (float)t [row] / tau);
				if (row == 1){
					print (V [0, i]);
				}
			}

		}
*///Space and Time Parameters
		float dx = 100f * Mathf.Pow (10, -6);

		for (float i = 0; i < maxVal; i += delta) {
			x.Add (i);
		}

		t = new ArrayList ();
		delta = .000001f;
		maxVal = .000025f;
		for (float i = delta; i < maxVal; i += delta) {
			t.Add (i);
		}

		float q0 = 1f;
		cm = 1;

		double[,] V = new double[t.Count, x.Count];

		for (int i = 0; i < x.Count; i++) {
			for (int row = 0; row < t.Count; row++) {
				V [row, i] = (q0 / (2 * cm * lamda * Mathf.Sqrt (Mathf.PI * (float)t [row] / tau))) * Mathf.Exp ((-1.0f * Mathf.Pow ((float)x [i] / lamda, 2)-4.0f * Mathf.Pow ((float)t [row] / tau, 2)) / (4.0f * (float)t [row] / tau));
			}

		}


		newMat = new double[t.Count, (x.Count - 1) * 2 + 1];

		for (int row = 0; row < t.Count; row++) {
			for (int col = 0; col < x.Count - 1; col++) {

				newMat [row, col+x.Count] = V [row, col+1];
				newMat [row, (x.Count - 1) - col] = V [row, col];
			}
			newMat [row, x.Count - 1] = V [row, 0];
		}
		double max_Val = V [0, 0];

		//find max
		for (int row = 0; row < t.Count; row++) {
			for (int col = 0; col < x.Count; col++) {
				if (V [row, col] > max_Val) {
					max_Val = V [row, col];
				}

			}
		}
		//normalize matrix
		for (int row = 0; row < t.Count; row++) {
			for (int col = 0; col < (x.Count - 1) * 2+1; col++) {
				newMat [row, col] = newMat [row, col] / max_Val;
			}
		}
		StartCoroutine (FadetoClear());

	}
	private IEnumerator FadetoClear()
	{
		yield return new WaitForSeconds (.5f);
		while (true) 
		{
			for (int row = 0; row < t.Count; row++) {
				for (int col = 0; col < (x.Count - 1) * 2 + 1; col++) {
					Color bandColor = new Color ((float)newMat [row, 11], 0, 1f - (float)newMat [row, 11], .75f);
					FinalBands [0].material.color = bandColor;

				}

				yield return new WaitForSeconds (.2f);
			}
		}
	}
}
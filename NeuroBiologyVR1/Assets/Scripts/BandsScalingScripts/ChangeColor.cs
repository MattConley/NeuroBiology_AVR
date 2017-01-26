using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]

public class ChangeColor : MonoBehaviour {

    // Grapher 1 Component
	[Range(10, 100)]
	public int resolution = 10;

	private int currentResolution;
	private ParticleSystem.Particle[] points;
    // End of Grapher 1

    public bool isStimulating;
	public float multiplier;
	public int rowi = 0;
	public int coli = 0;

	public ArrayList x;
	public ArrayList t;
	ArrayList v0 = new ArrayList();
	

	//public List<GameObject> Bands;
	public List<Renderer> FinalBands;

	//Cell Paremeters

	float Ri = 36f;                 //Ohms/cm
	float Cm = 1 * Mathf.Pow(10, -6);         //F/cm^2
	float Rm = 1 / (36 * Mathf.Pow(10, -3));  //cm^2/S (Ohms*cm^2)
	public double[,] newMat;
	public double[,] V;
	//Matrix 1:11
	//101:11

	static Text timeText;

	static float time = 0f;

	private float objScale = 1;

	public static float inc = 0.0f;
	void Start()
	{

		timeText = GameObject.FindGameObjectsWithTag("Timer")[0].GetComponent<Text>();

		//Setting Up Array
		//Bands = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Bands"));
		//foreach (GameObject band in Bands) {
			//FinalBands.Add (band.GetComponent<Renderer> ());
		//}
		
//Space and Time Parameters
		float dx = 100f * Mathf.Pow (10, -6);

		x = new ArrayList ();
		float delta = 0.00005f;
		float maxVal = 0.001f;
		for (float i = 0; i < maxVal; i += delta) {
			x.Add (i);
		}
		

		t = new ArrayList ();
		delta = .000001f;
		maxVal = .000020f;
		for (float i = delta; i < maxVal; i += delta) {
			t.Add (i);
		}

		float q0 = 1f;
		float diameter = Mathf.Pow(10, -6);   //Diameter
		float ri = 4f * Ri / (Mathf.PI * Mathf.Pow (diameter, 2f));
		float cm = Cm * Mathf.PI * diameter;
		float rm = Rm / (Mathf.PI * diameter);
		float lamda = 3f*Mathf.Sqrt (rm / ri)/2f; // Space Constant
		float tau = rm * cm; //Time Constant
		double max_Val = (q0 / (2 * cm * lamda * Mathf.Sqrt (Mathf.PI * (float)t [0] / tau))) * Mathf.Exp ((-1.0f * Mathf.Pow ((float)x [0] / lamda, 2)-4.0f * Mathf.Pow ((float)t [0] / tau, 2)) / (4.0f * (float)t [0] / tau));;

		diameter = multiplier * Mathf.Pow(10, -6);   //Diameter
		ri = 4f * Ri / (Mathf.PI * Mathf.Pow (diameter, 2f));
		cm = Cm * Mathf.PI * diameter;
		rm = Rm / (Mathf.PI * diameter);
		lamda = 3f*Mathf.Sqrt (rm / ri)/2f; // Space Constant
		tau = rm * cm; //Time Constant
		
		V = new double[t.Count, x.Count];

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
			newMat [row, 0] = V [row, x.Count - 1];
		}
		
		

		//normalize matrix
		for (int row = 0; row < t.Count; row++) {
			for (int col = 0; col < (x.Count - 1) * 2+1; col++) {
				newMat [row, col] = newMat [row, col] / max_Val;
				
			}
		}

		
		//Create New Points for Graph
		CreatePoints();
	}

	public void CreatePoints () {

		if (resolution < 10 || resolution > 100) {
			Debug.LogWarning("Grapher resolution out of bounds, resetting to minimum.", this);
			resolution = 10;
		}
		currentResolution = resolution;
		points = new ParticleSystem.Particle[resolution];
		float increment = 1f / (resolution - 1);
		for(int i = 0; i < resolution; i++){
			float x = i * 205f*increment;
			points[i].position = new Vector3(x, 0f, 0f);
			points[i].color = new Color(0f, 0f, 0f);
			points[i].size = 2f;
		}
	}

/*
	void Update(){

		if(Input.GetMouseButtonDown(0)){
			//isStimulating = false;
			isStimulating = true;
			rowi = 0;
			coli = 0;
			StartCoroutine (FadetoClear());
		}


	}
	*/

	public void OnTrigger(){

			isStimulating = true;
			rowi = 0;
			coli = 0;
			StartCoroutine (FadetoClear());


	}

	public void SetVariable(float value){

		multiplier = value;
		Start();

	}

	public IEnumerator FadetoClear()
	{

		while (isStimulating == true) 
		{
			for (rowi = 0; rowi < t.Count; rowi++) {
				for (coli = 0; coli < (x.Count - 1) * 2 + 1; coli++) {
					Color bandColor = new Color ((float)newMat [rowi, coli], 0, 1f - (float)newMat [rowi, coli], .75f);
					FinalBands [coli].material.color = bandColor;
					//Audio Component					
					/*
					AudioSource audio;
					audio = GetComponent<AudioSource>();
					audio.pitch = 1.05946f + (float)newMat [rowi,20];
					audio.Play();
*/
				}
               

                inc = (float)newMat [rowi,20];
                AudioSource audio;
                audio = GetComponent<AudioSource>();  
                if(inc>0){
                	audio.pitch = 1+Mathf.Lerp(0,(float)newMat [0,20], inc);
					audio.Play();
					inc += .5f * Time.deltaTime;
                }


				if (currentResolution != resolution) {
					CreatePoints();
				}

				for (int i = 0; i < resolution; i++) {
					Vector3 p = points[i].position;
					p.y = 10f*(float)newMat[rowi,i];
					points[i].position = p;
				}
			
				GetComponent<ParticleSystem>().SetParticles(points, points.Length);

				isStimulating = false;
				//timeText.text = Mathf.Round(rowi*delta*1e6f) + "μs";
				yield return new WaitForSeconds (.15f);
			}
		}
	}
}
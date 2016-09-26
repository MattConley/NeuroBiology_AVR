using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ChangeColor : MonoBehaviour {
    public Renderer band;
    /*public float rValue;
    public float gValue;
    public float bValue;*/

    public List<GameObject> Bands;
    public List<ChangeColor> Lattice;

    //Cell Paremeters
    float diameter = 2 * 10 ^ -6;   //Diameter
    float Ri = 36f;                 //Ohms/cm
    float Cm = 1 * 10 ^ -6;         //F/cm^2
    float Rm = 1 / (36 * 10 ^ -3);  //cm^2/S (Ohms*cm^2)

    //Matrix 1:11
    //101:11

    void Start()
    {
        Lattice = new List<ChangeColor>(FindObjectsOfType<ChangeColor>());

        float ri = 4f * Ri / (Mathf.PI * Mathf.Pow(diameter, 2f));
        float cm = Cm * Mathf.PI * diameter;
        float rm = Rm / (Mathf.PI * diameter);

        float lamda = Mathf.Sqrt(rm / ri); // Space Constant
        float tau = rm * cm; //Time Constant

        //Voltage over space
        float dx = 100f * Mathf.Pow(10,-6);

        // Voltage Over Time
        x = (0:dx:Mathf.Pow(1000,-6));
        V0 = 2f*Mathf.Exp(-x/lamda);   //Initial Voltage(over space)
        float dt = 1f * Mathf.Pow(10, -6);
        t = (0:dt: 100e-6);
        V = zeros(length(t), length(x));

        for( int i = 0; i < length(x); i++)
        {
            V(:, i) = V0(1, i) * exp(-t / tau);
        }
        // Enforcing Symmetry
        x = [fliplr(x(2:end)) x];
        V = [fliplr(V(:, 2:end)) V];

        //RGB Values 101:21
        R = V / max(max(V));
        

        Color bandColor = new Color(R, 0, 1-R, .75f);
        band.material.color = bandColor;

    }
    

    





   



}

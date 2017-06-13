using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*The purpose of this class was to function as a data structure to add new stimulus
 * to the current voltage.  There was a pre-calculated voltage curve which would be
 * scaled by the driving force equation, (Gsyn*(V - Esyn)) and then added to the existing
 * set of values. Unfortunately, The equation implemented with this data structure featured
 * a faulty combination of two differing methods, and as a result was not a true /
 * accurate simulation.  Currently, this class is not used, as the more accurate simulation 
 * has no need for it.
 */
public class ConveyerDS {

    public int mySize { get; private set; }
    private float[] values, defaultValues;
    private float baseVal;  //reset values to this after they are called
    private float timeStep; //each value is one timeStep later than the last
    private int currIndex;  //loops from 0 - values.length-1

	public ConveyerDS(int s, float bVal, float tStep)
    {
        mySize = s;
        baseVal = bVal;
        currIndex = 0;
        timeStep = tStep;
        values = new float[s];
        defaultValues = new float[s];
    }

    public void PreCompute(float inVolt, float dist)
    {
        float Ri = 36f;                 //Ohms/cm
        float Cm = 1 * Mathf.Pow(10, -6);         //F/cm^2
        float Rm = 1 / (36 * Mathf.Pow(10, -3));  //cm^2/S (Ohms*cm^2)
        float diameter = Mathf.Pow(10, -6);   //Diameter
        float ri = 4f * Ri / (Mathf.PI * Mathf.Pow(diameter, 2f));
        float cm = Cm * Mathf.PI * diameter;
        float rm = Rm / (Mathf.PI * diameter);
        float lambda = 3f * Mathf.Sqrt(rm / ri) / 2f; // Space Constant
        float tau = rm * cm; //Time Constant
        float q0 = 1f;
        float timeConst = .000002f;

        //compute the values of the default stimulation
        //find initial voltage at distance dist from the dendrite Eqn 14
        float initVolt = inVolt * Mathf.Exp(-1f * dist / lambda);
        for (int i = 0; i < mySize; i++)
        {
            values[i] = baseVal;

            //then vary voltage based on time Eqn 18
            defaultValues[i] = initVolt + (baseVal - initVolt)*(1 - Mathf.Exp((-1f * timeConst*i) / tau));
            //defaultValues[i] = (q0 / (2 * cm * lambda * Mathf.Sqrt(Mathf.PI 
            //    * (timeConst*(i+1)) / tau))) * Mathf.Exp((-1.0f * Mathf.Pow(dist / lambda, 2) - 4.0f * Mathf.Pow((timeConst * (i + 1)) / tau, 2)) / (4.0f * (timeConst * (i + 1)) / tau));
            //Debug.Log(defaultValues[i]);
        }
    }

    public float PopVal()   //returns the current value, shifting the conveyer belt
    {
        //get value at currIndex
        float val = values[currIndex];
        //reset value at currIndex
        values[currIndex] = baseVal;
        //increment currIndex
        currIndex = (currIndex + 1) % mySize;
        //return value
        return val;
    }

    public void DefaultStim(float scaleFactor)  //add the scaled voltage values
    {
        int valsIndex;
        for(int i = 0; i < mySize; i++)
        {
            valsIndex = (currIndex + i) % mySize;
            values[valsIndex] = values[valsIndex] + (defaultValues[i] - baseVal) * scaleFactor;
        }
    }

    public void ExternalAugment(float[] xVals) { }//for later use if dendrites have varying lengths, and therefore varying default values

}

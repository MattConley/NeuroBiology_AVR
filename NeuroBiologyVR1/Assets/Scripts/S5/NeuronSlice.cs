using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronSlice {

    public List<NeuronSlice> nextSlice, prevSlice;
    private float restVal, capVal, r_a;
    public float currentVal { get; private set; }
    public float currentU { get; private set; }
    public float lowVal { get; private set; }
    public float highVal { get; private set; }


    public bool isActive { get; private set; }

    private GameObject slice_obj;
    private Material slice_mat;
    private Color defColor;

    private struct SliceConnection
    {
        public float axial_resistance;
        NeuronSlice neiNeuron;
    }

    private struct InitParams
    {
        public float a;
        public float b;
        public float c;
        public float d;
    }

    private struct PostSynapse
    {
        public Synapse syn;
        public bool isExcite;
        public float eSynVal;
        public float gSynMax;
    }

    private InitParams myParams;

    private List<PostSynapse> mySynapses;       //not an array because of dynamic resizing

    private SliceConnection[] myConnections;

    public float[] CalcVoltageChange(float r_membrane, float appCurrent, List<NeuronSlice> neighborSlices)
    {
        float[] dRA = new float[2]; //[0] is dV, [1] is dU
        if (isActive)
        {
            dRA[0] = (currentVal * currentVal * 0.04f) + (5 * currentVal) + 140 - currentU;// + appCurrent;
            dRA[1] = myParams.a * (myParams.b * currentVal - currentU);
        }
        else
        {
            dRA[0] = (restVal - currentVal) / (r_membrane * capVal);
        }
        float neiVal = 0f;

        for(int i = 0; i < neighborSlices.Count; i++)
        {
            neiVal += (neighborSlices[i].currentVal - currentVal) / r_a;
        }
        dRA[0] += neiVal/capVal;

        dRA[0] += appCurrent;

        return dRA;
    }

    public float[] CalcVoltageChange_Active(float appCurrent) //also needs to calculate dU
    {
        float[] dRA = new float[2]; //[0] is dV, [1] is dU
                                            //.04f                      //140
        dRA[0] = (currentVal * currentVal * 0.04f) + (5 * currentVal) + 140 - currentU + appCurrent;
        dRA[1] = myParams.a * (myParams.b * currentVal - currentU);

        return dRA;
    }

    public void UpdateVal(float dV, float dT, float dU = 0f)
    {
        if(!isActive)
            currentVal += dV*dT;
        else
        {
            if(currentVal >= 30)
            {
                currentVal = myParams.c;
                currentU += myParams.d;
                
            }
            else
            {
                currentVal += dV * dT;
                if(currentVal >= 30)
                {
                    currentVal = 30f;
                }
                currentU += dU * dT;
            }
        }
    }

    

    public float GetVal()
    {
        return currentVal;
    }

    public float GetU()
    {
        return currentU;
    }

    public void UpdateColor()
    {
        slice_mat.color = Color.blue * (currentVal / 1f);
    }

    public NeuronSlice()
    {
        prevSlice = new List<NeuronSlice>();
        nextSlice = new List<NeuronSlice>();
    }
    //Constructor used for passive components
    public NeuronSlice(GameObject gObj, float lVal, float hVal, float rVal, float cVal, float raVal)
    {
        isActive = false;
        prevSlice = new List<NeuronSlice>();
        nextSlice = new List<NeuronSlice>();
        slice_obj = gObj;
        slice_mat = gObj.GetComponent<MeshRenderer>().material;
        defColor = slice_obj.GetComponent<MeshRenderer>().material.color;
        lowVal = lVal;
        highVal = hVal;
        restVal = rVal;
        currentVal = rVal;
        capVal = cVal;
        r_a = raVal;
    }
    //Active compartment constructor
    public NeuronSlice(GameObject gObj, float lVal, float hVal, float initU, float initV, float aVar, float bVar, float cVar, float dVar, float capacitanceVal, float raVal)
    {
        isActive = true;
        prevSlice = new List<NeuronSlice>();
        nextSlice = new List<NeuronSlice>();
        slice_obj = gObj;
        slice_mat = gObj.GetComponent<MeshRenderer>().material;
        defColor = slice_obj.GetComponent<MeshRenderer>().material.color;
        lowVal = lVal;
        highVal = hVal;
        currentU = initU;
        currentVal = initV;
        myParams.a = aVar;
        myParams.b = bVar;
        myParams.c = cVar;
        myParams.d = dVar;
        capVal = capacitanceVal;
        r_a = raVal;
    }
    //These following methods should be implemented in the future to improve efficiency
    //Right now the neighbors are calculated based on index, and only for the large neuron
    public void AddNextSlice(NeuronSlice new_slice)
    {
        nextSlice.Add(new_slice);
    }

    public void AddPrevSlice(NeuronSlice new_slice)
    {
        prevSlice.Add(new_slice);
    }

    public void SetVoltage(float newV)
    {
        currentVal = newV;
    }

}

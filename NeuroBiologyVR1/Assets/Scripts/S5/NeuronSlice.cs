using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronSlice {

    public List<NeuronSlice> nextSlice, prevSlice;
    private float restVal, capVal, r_a;
    public float currentVal { get; private set; }
    public float currentU { get; private set; }

    public bool isActive { get; private set; }

    private GameObject slice_obj;
    private Material slice_mat;
    private Color defColor;

    private struct InitParams
    {
        public float a;
        public float b;
        public float c;
        public float d;
    }

    private InitParams myParams;

    public float CalcVoltageChange(float r_membrane, float appCurrent)
    {
        float dV = 0f;

        dV += (restVal - currentVal) / r_membrane;

        float avgVal = 0f;

        /*if (prevSlice.Count > 0)
        {
            for (int i = 0; i < prevSlice.Count; i++)
            {
                avgVal += (prevSlice[i].currentVal - currentVal) / r_a;
            }
            dV += avgVal / prevSlice.Count;
            Debug.Log(prevSlice[0].currentVal);
            //Debug.Log(currentVal);
        }*/
        avgVal = 0f;
        if (nextSlice.Count > 0)
        {
            for (int i = 0; i < nextSlice.Count; i++)
            {
                avgVal += (nextSlice[i].currentVal - currentVal) / r_a;
            }
            dV += avgVal / nextSlice.Count;
            //Debug.Log(nextSlice[0].currentVal);
            //Debug.Log(avgVal);
        }

        dV += appCurrent;

        dV = dV / capVal;

        //currentVal += dV;
        if (dV != 0)
        {
            Debug.Log(avgVal);
            Debug.Log(dV);
        }

        return dV;
    }

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
        /*
        if(currentVal >= 30)
        {
            currentVal = myParams.c;
            currentU += myParams.d;
        }/**/
        float[] dRA = new float[2]; //[0] is dV, [1] is dU
        //float dU = 0f;
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
            if(currentVal >= 30)//30
            //if (currentVal + dV >= 30)
            {
                
                //currentVal = 30;
                //currentU += dU * dT;
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

    public NeuronSlice(GameObject gObj, float rVal, float cVal, float raVal, List<NeuronSlice> pSlices)
    {
        isActive = false;
        prevSlice = new List<NeuronSlice>();
        nextSlice = new List<NeuronSlice>();
        slice_obj = gObj;
        slice_mat = gObj.GetComponent<MeshRenderer>().material;
        defColor = slice_obj.GetComponent<MeshRenderer>().material.color;
        restVal = rVal;
        currentVal = rVal;
        capVal = cVal;
        r_a = raVal;

        for(int i = 0; i < pSlices.Count; i++)
        {
            prevSlice.Add(pSlices[i]);
        }

    }

    public NeuronSlice(GameObject gObj, float rVal, float cVal, float raVal)
    {
        isActive = false;
        prevSlice = new List<NeuronSlice>();
        nextSlice = new List<NeuronSlice>();
        slice_obj = gObj;
        slice_mat = gObj.GetComponent<MeshRenderer>().material;
        defColor = slice_obj.GetComponent<MeshRenderer>().material.color;
        restVal = rVal;
        currentVal = rVal;
        capVal = cVal;
        r_a = raVal;
    }

    public NeuronSlice(GameObject gObj, float initU, float initV, float aVar, float bVar, float cVar, float dVar, float capacitanceVal, float raVal)
    {
        isActive = true;
        prevSlice = new List<NeuronSlice>();
        nextSlice = new List<NeuronSlice>();
        slice_obj = gObj;
        slice_mat = gObj.GetComponent<MeshRenderer>().material;
        defColor = slice_obj.GetComponent<MeshRenderer>().material.color;
        currentU = initU;
        currentVal = initV;
        myParams.a = aVar;
        myParams.b = bVar;
        myParams.c = cVar;
        myParams.d = dVar;
        capVal = capacitanceVal;
        r_a = raVal;
    }

    public void AddNextSlice(NeuronSlice new_slice)
    {
        nextSlice.Add(new_slice);
    }

    public void AddPrevSlice(NeuronSlice new_slice)
    {
        prevSlice.Add(new_slice);
    }
}

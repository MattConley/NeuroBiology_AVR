using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class VoltageUpdate : MonoBehaviour {

    public GameObject[] slice_objects;

    private float[] slice_volts, slice_v2;

    private float r_membrane, capacitance, v_rest, r_a, v_applied;
    private float timestep = .1f;//0.002f;

    private int numSlices, stimPos;
    private bool isStim = false;

    public bool printGraph;
    private StreamWriter graphWriter;
    private string stringValues = "";

    private float aVal, bVal, cVal, dVal, uVal;

    public List<NeuronSlice> neuron_slices;// { get;  private set; }

    // Use this for initialization
    void Start()
    {
        //Debug.Log(slice_objects[0]);
        //set values
        v_rest = -65f;      //-65f
        r_membrane = 10f;   //1f
        capacitance = 1f;
        v_applied = 10f;
        r_a = 5f;           //1f
        stimPos = 10;

        //regular spiking values
        aVal = .02f;    //.02f
        bVal = 0.2f;    //.2f
        cVal = -65f;    //-65f
        dVal = 8f;      //8f
        uVal = -13f;    //-13f

        //instantiate list
        neuron_slices = new List<NeuronSlice>();
        numSlices = slice_objects.Length;
        if(printGraph)
            graphWriter = new StreamWriter(@"C:\Users\mattc\Desktop\GraphText\Full_Gamut.txt", true);

        //stringValues = new string[numSlices];


        int curLen = 0;
        int i;
        for (int k = 0; k < numSlices; k++) //(int k = numSlices - 1; k >= 0; k--)
        {
            i = k;// 19 - k;
            curLen = neuron_slices.Count;
            NeuronSlice t_slice = new NeuronSlice();
            List<NeuronSlice> prevs = new List<NeuronSlice>();
            if(i == 0) {
                t_slice = new NeuronSlice(slice_objects[i], v_rest, capacitance, r_a);
                //t_slice.AddPrevSlice(neuron_slices[curLen - 1]);
            }
            else if (i <= 8) //active bands
            {
                t_slice = new NeuronSlice(slice_objects[i], uVal, v_rest, aVal, bVal, cVal, dVal, capacitance, r_a);
                //t_slice.AddPrevSlice(neuron_slices[curLen - 1]);
                //neuron_slices[curLen - 1].AddNextSlice(t_slice);
            }
            else
            {
                //prevs.Add(neuron_slices[curLen - 1]);
                t_slice = new NeuronSlice(slice_objects[i], v_rest, capacitance, r_a);
                //t_slice.AddPrevSlice(neuron_slices[curLen - 1]);
                //neuron_slices[curLen - 1].AddNextSlice(t_slice);
            }
            neuron_slices.Add(t_slice);
        }
        Debug.Log("Finished Start");
    }

    private void OnDestroy()
    {
        if(printGraph)
            graphWriter.Close();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void FixedUpdate()
    {
        float[] newVals = new float[numSlices];
        float[] newUs = new float[numSlices];
        if (Input.GetKey(KeyCode.Space))
        {
            isStim = true;
            Debug.Log("Stimulated");
        }
        bool shouldDebug = Input.GetKeyUp(KeyCode.Space);
        for(int k = 0; k < numSlices; k++)
        {
            int i = k;// numSlices - (k + 1);
            float applied_current = ((isStim && (i == stimPos)) || neuron_slices[i].isActive) ? v_applied : 0f;
            Debug.Log(applied_current);
            NeuronSlice[] pRA = new NeuronSlice[2]; //2 is current max neighbors
            NeuronSlice[] nRA = new NeuronSlice[2];
            List<NeuronSlice> neighborList = new List<NeuronSlice>();
            if (i > 12)
            {
                neighborList.Add(neuron_slices[i - 2]);

                if (i < numSlices - 2)
                { 
                    neighborList.Add(neuron_slices[i + 2]);
                }
            }
            else if (i == 12) //next is i+1, not i+2
            {
                neighborList.Add(neuron_slices[i + 2]);
                neighborList.Add(neuron_slices[i - 1]);
            }
            else if (i==11)//(i == 8) //two prevs: i-1, i-2
            {
                neighborList.Add(neuron_slices[i + 2]);
                neighborList.Add(neuron_slices[i + 1]);
                neighborList.Add(neuron_slices[i - 1]);
            }     
            else //prev and next are +/- 1
            {
                /*if (neuron_slices[i].isActive)
                {
                    //float acur = isStim ? 20f : 0f;
                    float acur = 10f;
                    float[] deltas = neuron_slices[i].CalcVoltageChange_Active(acur);
                    newVals[i] = deltas[0];
                    newUs[i] = deltas[1];
                    Debug.Log(i);
                }
                else
                {*/
                    neighborList.Add(neuron_slices[i + 1]);
                    
                    if(i != 0)
                        neighborList.Add(neuron_slices[i - 1]);

                //}
            }
            float[] retResult = neuron_slices[i].CalcVoltageChange(r_membrane, applied_current, neighborList);
            newVals[i] = retResult[0];
            if (neuron_slices[i].isActive)
                newUs[i] = retResult[1];
        }
        isStim = false;
        stringValues = "";
        string stringUValues = "";
        for (int i = 0; i < numSlices; i++)
        {
            if (neuron_slices[i].isActive)
            {
                neuron_slices[i].UpdateVal(newVals[i], timestep, newUs[i]);
                /*if (printGraph)
                {
                    stringValues = stringValues + neuron_slices[i].GetVal() + " ";
                }*/
            } else
                neuron_slices[i].UpdateVal(newVals[i], timestep);
            slice_objects[i].GetComponent<MeshRenderer>().material.color = Color.blue * (neuron_slices[i].GetVal());
            if (printGraph)
            {
                stringValues = stringValues + neuron_slices[i].GetVal() + " ";
                if (neuron_slices[i].isActive)
                    stringUValues = stringUValues + neuron_slices[i].GetU() + " ";
            }
        }

        

        //Debug.Log(neuron_slices[11].GetU());

        if (printGraph)
        {
            graphWriter.WriteLine(stringValues + stringUValues + "\n");
            //graphWriter.Flush
        }
        //Debug.Log(neuron_slices[10].GetVal());
    }
}

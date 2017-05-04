using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.IO;


public class VoltageUpdate : MonoBehaviour {

    public GameObject[] slice_objects;

    private float[] slice_volts, slice_v2;

    private float r_membrane, capacitance, v_rest, r_a, v_applied;
    private float timestep = .1f;//0.002f;

    private int numSlices, stimPos;
    private bool isStim = false;

    public bool printGraph;
    //private StreamWriter graphWriter;
    private string stringValues = "";

    private float aVal, bVal, cVal, dVal, uVal;

    private float lowVal_act = -65f, highVal_act = 30f;
    private float lowVal_pass = -65f, highVal_pass = -20f;

    public List<NeuronSlice> neuron_slices;// { get;  private set; }

    // Use this for initialization
    void Start()
    {
        Time.fixedDeltaTime = 0.01f;
        //Debug.Log(slice_objects[0]);
        //set values
        v_rest = -65f;      //-65f
        r_membrane = 20f;   //10f
        capacitance = 1f;
        v_applied = 30f;
        r_a = 1f;           //1f
        stimPos = 10;

        //Initial u = b*v_rest

        /*//regular spiking values
        aVal = .02f;    //.02f
        bVal = 0.2f;    //.2f
        cVal = -65f;    //-65f
        dVal = 8f;      //8f
        uVal = -13f;    //-13f
        /**/

        //fast spiking values
        aVal = .1f;    //.1f
        bVal = 0.2f;    //.2f
        cVal = -65f;    //-65f
        dVal = 2f;      //8f
        uVal = -13f;    //-13f

        //instantiate list
        neuron_slices = new List<NeuronSlice>();
        numSlices = slice_objects.Length;

        //if(printGraph)
          //  graphWriter = new StreamWriter(@"C:\Users\mattc\Desktop\GraphText\Full_Gamut.txt", true);

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
                t_slice = new NeuronSlice(slice_objects[i], lowVal_pass, highVal_pass, v_rest, capacitance, r_a);
                //t_slice.AddPrevSlice(neuron_slices[curLen - 1]);
            }
            else if (i == 8) //active bands
            {
                t_slice = new NeuronSlice(slice_objects[i], lowVal_act, highVal_act, uVal, v_rest, aVal, bVal, cVal, dVal, capacitance, r_a+1f);
            }
            else if (i < 8) //active bands
            {
                t_slice = new NeuronSlice(slice_objects[i], lowVal_act, highVal_act, uVal, v_rest, aVal, bVal, cVal, dVal, capacitance, r_a);
            }
            else
            {
                //prevs.Add(neuron_slices[curLen - 1]);
                t_slice = new NeuronSlice(slice_objects[i], lowVal_pass, highVal_pass, v_rest, capacitance, r_a);
                //t_slice.AddPrevSlice(neuron_slices[curLen - 1]);
                //neuron_slices[curLen - 1].AddNextSlice(t_slice);
            }
            neuron_slices.Add(t_slice);
        }
        Debug.Log("Finished Start");
    }

    private void OnDestroy()
    {
        //if(printGraph)
        //    graphWriter.Close();
    }

    // Update is called once per frame
   /* void Update () {
		
	}*/

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
            //float applied_current = ((isStim && (i == stimPos)) || neuron_slices[i].isActive) ? v_applied : 0f;
            float applied_current = (isStim && (i == stimPos)) ? v_applied : 0f;
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
            else //if (!neuron_slices[i].isActive) //prev and next are +/- 1
            {
                 
                neighborList.Add(neuron_slices[i + 1]);                    
                if(i != 0)
                {
                    if (!neuron_slices[i].isActive)
                        neighborList.Add(neuron_slices[i - 1]);
                }
                    //neighborList.Add(neuron_slices[i - 1]);
            }
            float[] retResult = neuron_slices[i].CalcVoltageChange(r_membrane, applied_current, neighborList);
            newVals[i] = retResult[0];
            if (neuron_slices[i].isActive)
                newUs[i] = retResult[1];
        }
        isStim = false;
        stringValues = "";
        //string stringUValues = "";
        //float lowVal, highVal;
        for (int i = 0; i < numSlices; i++)
        {
            if (neuron_slices[i].isActive)
            {
                neuron_slices[i].UpdateVal(newVals[i], timestep, newUs[i]);
                slice_objects[i].GetComponent<MeshRenderer>().material.color = Color.red * ((neuron_slices[i].GetVal()) - neuron_slices[i].lowVal) / (neuron_slices[i].highVal - neuron_slices[i].lowVal);
                /*if (printGraph)
                {
                    stringValues = stringValues + neuron_slices[i].GetVal() + " ";
                }*/
            }
            else
            {
                neuron_slices[i].UpdateVal(newVals[i], timestep);
                slice_objects[i].GetComponent<MeshRenderer>().material.color = Color.blue * ((neuron_slices[i].GetVal()) - neuron_slices[i].lowVal) / (neuron_slices[i].highVal - neuron_slices[i].lowVal);
            }
            /*if (printGraph)
            {
                stringValues = stringValues + neuron_slices[i].GetVal() + " ";
                if (neuron_slices[i].isActive)
                    stringUValues = stringUValues + neuron_slices[i].GetU() + " ";
            }*/
        }

      
        //if (printGraph)
          //  graphWriter.WriteLine(stringValues + stringUValues + "\n");
        
    }

    

}

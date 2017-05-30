using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.IO;


public class VoltageUpdate : MonoBehaviour {


    public GameObject[] slice_objects;
    public GameObject neuron_obj;
    public GameObject pulsePrefab;

    //public int numDendrites;
    public bool isStimmable;
    public int rcIndex, triggerIndex;
    public int[] denIndices;

    public LinkManager lMan;

    private float[] slice_volts, slice_v2;

    private float r_membrane, capacitance, v_rest, r_a, v_applied;
    private float timestep = .1f;// number of seconds? per update
    private float fadeMiliSeconds = 10;

    private int numSlices, stimPos;
    private bool isStim = false;

    public bool printGraph;
    //private StreamWriter graphWriter;
    private string stringValues = "";

    private float aVal, bVal, cVal, dVal, uVal;

    private float lowVal_act = -65f, highVal_act = 30f;
    private float lowVal_pass = -65f, highVal_pass = -20f;

    private List<ActionPotential> apList = new List<ActionPotential>();
    private List<Synapse> synList = new List<Synapse>();

    public List<NeuronSlice> neuron_slices;// { get;  private set; }

    public ConveyerDS rcVals { get; private set; }
    private bool doneTmp = false;

    public int neuronID;

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
            if(i == rcIndex)
            {
                t_slice = new NeuronSlice(slice_objects[i], lowVal_pass, highVal_pass, v_rest, capacitance, r_a);
            }
            else if(i == triggerIndex)
            {
                t_slice = new NeuronSlice(slice_objects[i], lowVal_act, highVal_act, uVal, v_rest, aVal, bVal, cVal, dVal, capacitance, r_a + 1f);
            }
            else
            {
                t_slice = new NeuronSlice(slice_objects[i], lowVal_pass, highVal_pass, v_rest, capacitance, r_a);
            }
            /*//old, fully simulated model
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
            }*/
            neuron_slices.Add(t_slice); //neuron_slices will contain all slices to have access to their renderers
        }//                         710
        //synList.Add(new Synapse(30, 30, -65, neuron_slices[0], neuron_slices[denIndices[0]], .002f));
        //synList.Add(new Synapse(30, 30, -65, neuron_slices[0], neuron_slices[denIndices[1]], .002f));
        int resolution = (int)(fadeMiliSeconds / timestep);
        rcVals = new ConveyerDS(resolution, v_rest, timestep);
        rcVals.PreCompute(v_applied, .00015f);
        
        //Debug.Log("Finished Start");
    }

    public void AddSynapse(Synapse newSyn)
    {
        Debug.Log("Added");
        synList.Add(newSyn);
    }

    public GameObject GetObject(int objIndex)
    {
        return slice_objects[objIndex];
    }

    public int GetDenIndex(int dIndex)
    {
        return denIndices[dIndex];
    }

    public NeuronSlice GetSlice(int sliceIndex)
    {
        Debug.Log(neuron_slices.Count);
        return neuron_slices[sliceIndex];
    }

    private void OnDestroy()
    {
        //if(printGraph)
        //    graphWriter.Close();
    }

    private void FixedUpdate()
    {
        if (!doneTmp)
        {
            if (!isStimmable)
            {
                switch (neuronID)
                {
                    case 3:
                        lMan.CreateTestSynapse2(neuron_slices[denIndices[0]], rcVals);
                        break;
                    default:
                        lMan.CreateTestSynapse(neuron_slices[denIndices[0]], rcVals);
                        //lMan.CreateTestSynapse3(neuron_slices[denIndices[1]], rcVals);
                        break;
                }
            }
            else
            {
                lMan.CreateTestSynapse2(neuron_slices[denIndices[0]], rcVals);
                //lMan.CreateTestSynapse3(neuron_slices[denIndices[0]], rcVals);
            }
            doneTmp = true;
        }
        float[] newVals = new float[numSlices];
        float[] newUs = new float[numSlices];
        float[] retResult;
        if (Input.GetKey(KeyCode.Space))
        {
            isStim = true;
        }
        bool shouldDebug = Input.GetKeyUp(KeyCode.Space);

        /*for (int k = 0; k < numSlices; k++)
        {
            int i = k;// numSlices - (k + 1);
            //float applied_current = ((isStim && (i == stimPos)) || neuron_slices[i].isActive) ? v_applied/4 : 0f;
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
            else if (i==11) //two prevs: i-1, i-2
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
            retResult = neuron_slices[i].CalcVoltageChange(r_membrane, applied_current, neighborList);
            newVals[i] = retResult[0];
            if (neuron_slices[i].isActive)
                newUs[i] = retResult[1];
        }*/
        float applied_current = (isStim && isStimmable) ? v_applied : 0f;
        List<NeuronSlice> neighborList = new List<NeuronSlice>();
        neighborList.Add(neuron_slices[rcIndex]);
        retResult = neuron_slices[triggerIndex].CalcVoltageChange(r_membrane, applied_current, neighborList);
        newVals[triggerIndex] = retResult[0];
        newUs[triggerIndex] = retResult[1];
        neighborList = new List<NeuronSlice>();

        for (int i = 0; i < denIndices.Length; i++)
        {
            newVals[denIndices[i]] = neuron_slices[denIndices[i]].CalcVoltageChange(r_membrane, 0f, neighborList)[0];
        }

        /*rc junction is now pre calculated
        neighborList = new List<NeuronSlice>();
        retResult = neuron_slices[rcIndex].CalcVoltageChange(r_membrane, applied_current, neighborList);
        newVals[rcIndex] = retResult[0];
        */

        isStim = false;
        stringValues = "";
        //string stringUValues = "";
        for(int i = apList.Count - 1; i >= 0; i--)
        {
            //Debug.Log("AP Here\n");
            if (apList[i].ShouldDespawn(timestep))
            {
                TriggerSynapes();
                apList[i].Destroy();
                apList.RemoveAt(i);
            }
        }

        /*for (int i = 0; i < numSlices; i++)
        {
            if (neuron_slices[i].isActive)
            {
                neuron_slices[i].UpdateVal(newVals[i], timestep, newUs[i]);
                if (i == 8 && neuron_slices[i].currentVal >= highVal_act)
                {
                    //make new ActionPotential                                                                                                                          m/s m
                    ActionPotential newAP = new ActionPotential(neuron_obj, pulsePrefab, slice_objects[i].transform.position, slice_objects[i].transform.rotation, 1.7f, 7.5f);
                    //add the AP to the list
                    apList.Add(newAP);
                }
                //if (printGraph)
                //{
                //    stringValues = stringValues + neuron_slices[i].GetVal() + " ";
                //}
            }
            else
            {
                neuron_slices[i].UpdateVal(newVals[i], timestep);
            }
            slice_objects[i].GetComponent<MeshRenderer>().material.color = Color.blue * Mathf.Log10((neuron_slices[i].GetVal()) - neuron_slices[i].lowVal) / Mathf.Log10(neuron_slices[i].highVal - neuron_slices[i].lowVal);
            //if (printGraph)
            //{
            //    stringValues = stringValues + neuron_slices[i].GetVal() + " ";
            //    if (neuron_slices[i].isActive)
            //        stringUValues = stringUValues + neuron_slices[i].GetU() + " ";
            //}
        }*/

        neuron_slices[triggerIndex].UpdateVal(newVals[triggerIndex], timestep, newUs[triggerIndex]);
        for(int i = 0; i < denIndices.Length; i++)
        {
            neuron_slices[denIndices[i]].UpdateVal(newVals[denIndices[i]], timestep);
            slice_objects[denIndices[i]].GetComponent<MeshRenderer>().material.color = Color.blue * Mathf.Log10((neuron_slices[denIndices[i]].GetVal()) - neuron_slices[denIndices[i]].lowVal)
            / Mathf.Log10(neuron_slices[denIndices[i]].highVal - neuron_slices[denIndices[i]].lowVal);
        }
        if(neuron_slices[triggerIndex].currentVal >= highVal_act)
        {
            float axonSize;
            switch (neuronID)
            {
                case 3:
                    axonSize = 4.5f;
                    break;
                default:
                    axonSize = 7.5f;
                    break;
            }
            //make new ActionPotential                                                                                                                          m/s m
            ActionPotential newAP = new ActionPotential(neuron_obj, pulsePrefab, slice_objects[triggerIndex].transform.position, slice_objects[triggerIndex].transform.rotation, 1.7f, axonSize);
            //add the AP to the list
            apList.Add(newAP);
        }
        //neuron_slices[rcIndex].UpdateVal(newVals[rcIndex], timestep);
        float tVal = rcVals.PopVal();
        neuron_slices[rcIndex].SetVoltage(tVal);
        //Debug.Log(tVal);

        slice_objects[triggerIndex].GetComponent<MeshRenderer>().material.color = Color.blue * Mathf.Log10((neuron_slices[triggerIndex].GetVal()) - neuron_slices[triggerIndex].lowVal) 
            / Mathf.Log10(neuron_slices[triggerIndex].highVal - neuron_slices[triggerIndex].lowVal);

        slice_objects[rcIndex].GetComponent<MeshRenderer>().material.color = Color.blue * Mathf.Log10((neuron_slices[rcIndex].GetVal()) - neuron_slices[rcIndex].lowVal) 
            / Mathf.Log10(neuron_slices[rcIndex].highVal - neuron_slices[rcIndex].lowVal);

        //if (printGraph)
        //  graphWriter.WriteLine(stringValues + stringUValues + "\n");

    }
    public void TriggerSynapes()
    {
        for(int s = 0; s < synList.Count; s++)
        {
            synList[s].Trigger();
        }
    }
    

}

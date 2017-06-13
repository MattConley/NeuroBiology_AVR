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

    //Active neuron parameters
    private float aVal, bVal, cVal, dVal, uVal;

    //values which represent the current max and min voltages achieved by each neruon slice
    private float lowVal_act = -65f, highVal_act = 30f;
    private float lowVal_pass = -65f, highVal_pass = -20f;

    //list of active Action Potentials
    private List<ActionPotential> apList = new List<ActionPotential>();
    //List of active Synapses
    private List<Synapse> synList = new List<Synapse>();

    //list of cells that compose the Neuron, each of which is in a NeuronSlice data structure
    public List<NeuronSlice> neuron_slices;// { get;  private set; }

    //This is the code responsible for keeping track of the summing junction's voltage
    public SummingJunction rcJunction { get; private set; }
    private bool doneTmp = false;

    public int neuronID;

    // Use this for initialization
    void Start()
    {
        //sets the amount of time between FixedUpdate calls; change to improve framerate
        Time.fixedDeltaTime = 0.01f;
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
        dVal = 2f;      //2f
        uVal = -13f;    //-13f

        //instantiate list
        neuron_slices = new List<NeuronSlice>();
        numSlices = slice_objects.Length;

        //This code is used to output the voltages of the given neuron into a textfile
        //NOTE: with multiple neurons, the output file of graphWriter should change, possibly to use 
            //the Neuron's ID?
        //if(printGraph)
          //  graphWriter = new StreamWriter(@"C:\Users\mattc\Desktop\GraphText\Full_Gamut.txt", true);
        //this is to contain the values to be printed
        //stringValues = new string[numSlices];

        int curLen = 0;
        for (int i = 0; i < numSlices; i++) 
        {
            curLen = neuron_slices.Count;
            NeuronSlice t_slice = new NeuronSlice();
            //The only active compartment is the trigger zone, all others are added as passive by default
            //TODO change the NeuronSlice class to accept a list/array of other NeuronSlices as the current object's neighbors
            //Then neighborIn
            if(i == triggerIndex)
            {
                t_slice = new NeuronSlice(slice_objects[i], lowVal_act, highVal_act, uVal, v_rest, aVal, bVal, cVal, dVal, capacitance, r_a + 1f);
            }
            else
            {
                t_slice = new NeuronSlice(slice_objects[i], lowVal_pass, highVal_pass, v_rest, capacitance, r_a);
            }
            neuron_slices.Add(t_slice); //neuron_slices will contain all slices to have access to their renderers
        }
        
        int resolution = (int)(fadeMiliSeconds / timestep);

        //Needed for Tau; these values calculate the same Tau as in the cable simulatiion
        float Ri = 36f;                 //Ohms/cm
        float Cm = 1 * Mathf.Pow(10, -6);         //F/cm^2
        float Rm = 1 / (36 * Mathf.Pow(10, -3));  //cm^2/S (Ohms*cm^2)
        float diameter = Mathf.Pow(10, -6);   //Diameter
        float ri = 4f * Ri / (Mathf.PI * Mathf.Pow(diameter, 2f));
        float cm = Cm * Mathf.PI * diameter;
        float rm = Rm / (Mathf.PI * diameter);
        float lambda = 3f * Mathf.Sqrt(rm / ri) / 2f; // Space Constant
        float tau = rm * cm; //Time Constant

        Debug.Log(tau);
        Debug.Log(timestep);

        //The Srinivasan Chiel model scales the timestep by the time constant tau
        //the timestep is multiplied by tau because the current value is not of the right scale.
        //my understanding is that the time constant (tau) is normally responsible
        //for this scale; Essentially this combination seemed to produce reasonable results
        rcJunction = new SummingJunction(0, 0, tau, v_rest, timestep*tau);
        
    }

    //used to keep track of a new synapses by adding them to the post synaptic
    //neuron's list
    public void AddSynapse(Synapse newSyn)
    {
        synList.Add(newSyn);
    }

    //returns the gameobject at the specified index, used for changing color
    public GameObject GetObject(int objIndex)
    {
        return slice_objects[objIndex];
    }

    //Returns the neuronslice index of the specified dendrite
    public int GetDenIndex(int dIndex)
    {
        return denIndices[dIndex];
    }

    //returns the neuron slice at the specified index
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

    //Currently this method is designed specifically for the large neurons, indices would have to 
    //change/be generalized to improve compatibility (Neighborlist is populated based in the cell's
    //index in the array of the scene, for instance, which would vary with different length neurons)
    private void FullSimDelta(float[] newVals, float[] newUs, float[] retResult)
    {
        for (int k = 0; k < numSlices; k++)
        {
            int i = k;
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
            else if (i == 11) //two prevs: i-1, i-2
            {

                neighborList.Add(neuron_slices[i + 2]);
                neighborList.Add(neuron_slices[i + 1]);
                neighborList.Add(neuron_slices[i - 1]);

            }
            else //if (!neuron_slices[i].isActive) //prev and next are +/- 1
            {

                neighborList.Add(neuron_slices[i + 1]);
                if (i != 0)
                {
                    if (!neuron_slices[i].isActive)
                        neighborList.Add(neuron_slices[i - 1]);
                }
            }
            retResult = neuron_slices[i].CalcVoltageChange(r_membrane, applied_current, neighborList);
            newVals[i] = retResult[0];
            if (neuron_slices[i].isActive)
                newUs[i] = retResult[1];
        }

        for (int i = 0; i < numSlices; i++)
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
        }
    }

    public void ShortSim(float[] newVals, float[] newUs, float[] retResult)
    {
        float applied_current = (isStim && isStimmable) ? v_applied : 0f;
        List<NeuronSlice> neighborList = new List<NeuronSlice>();
        neighborList.Add(neuron_slices[rcIndex]);
        retResult = neuron_slices[triggerIndex].CalcVoltageChange(r_membrane, applied_current, neighborList);
        newVals[triggerIndex] = retResult[0];
        newUs[triggerIndex] = retResult[1];
        neighborList = new List<NeuronSlice>();

        /*rc junction is now not pre calculated*/
        rcJunction.UpdateSums();
        neighborList = new List<NeuronSlice>();
        neuron_slices[rcIndex].SetVoltage(rcJunction.GetVoltage());
        retResult = neuron_slices[rcIndex].CalcVoltageChange(r_membrane, 0f, neighborList);
        newVals[rcIndex] = retResult[0];
        /**/

        isStim = false;
        stringValues = "";

        neuron_slices[triggerIndex].UpdateVal(newVals[triggerIndex], timestep, newUs[triggerIndex]);
        if (neuron_slices[triggerIndex].currentVal >= highVal_act)
        {
            //Debug.Log(neuron_slices[triggerIndex].currentVal);
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


        neuron_slices[rcIndex].UpdateVal(newVals[rcIndex], timestep);

        slice_objects[triggerIndex].GetComponent<MeshRenderer>().material.color = Color.blue * Mathf.Log10((neuron_slices[triggerIndex].GetVal()) - neuron_slices[triggerIndex].lowVal)
            / Mathf.Log10(neuron_slices[triggerIndex].highVal - neuron_slices[triggerIndex].lowVal);

        slice_objects[rcIndex].GetComponent<MeshRenderer>().material.color = Color.blue * Mathf.Log10((neuron_slices[rcIndex].GetVal()) - neuron_slices[rcIndex].lowVal)
            / Mathf.Log10(neuron_slices[rcIndex].highVal - neuron_slices[rcIndex].lowVal);

        rcJunction.SetVoltage(neuron_slices[rcIndex].currentVal);
    }

    private void FixedUpdate()
    {
        //used to initialize connectivity; this block ensures synapses are created after start
        //If they are at the end of the start method, an error is sent
        if (!doneTmp)
        {
            if (!isStimmable)
            {
                switch (neuronID)
                {
                    case 3:
                        lMan.CreateTestSynapse2(neuron_slices[denIndices[0]], rcJunction, 55f);
                        break;
                    default:
                        lMan.CreateTestSynapse3(neuron_slices[denIndices[0]], rcJunction, 55f);
                        break;
                }
            }
            else
            {
                lMan.CreateTestSynapse(neuron_slices[denIndices[0]], rcJunction, 55f);
            }
            doneTmp = true;
        }
        //these arrays hold the derivative values which are later added into the main voltages
        float[] newVals = new float[numSlices];
        float[] newUs = new float[numSlices];
        float[] retResult = new float[2];

        //checks to see if the user is stimulating a neuron
        if (Input.GetKey(KeyCode.Space))
        {
            isStim = true;
        }

        //Currently, the synapses are triggered whenever an Action Potential reaches the end
        //of an axon; if we want this behavior to change in the future, maybe we can model the
        //end of the axon as a passive component, and give it a special trigger instead
        for (int i = apList.Count - 1; i >= 0; i--)
        {
            if (apList[i].ShouldDespawn(timestep))
            {
                TriggerSynapes();
                apList[i].Destroy();
                apList.RemoveAt(i);
            }
        }


        //To switch between fully simulated and the shortcut model; currently the synapses
        //are set up to work with the shortcut model, and would need to be changed as well.
        //FullSimUpdate(newVals, newUs, retResult);
        ShortSim(newVals, newUs, retResult);
        

        //More code to print the voltages
        //if (printGraph)
        //  graphWriter.WriteLine(stringValues + stringUValues + "\n");

    }
    //Triggers all synapses associated with the Neuron.  This might be an issue if the synapses
    //are associated with their dendritic neurons instead of the pre synaptic ones.  In order to
    //fix this potential problem, the synapses could only be added to the pre-synaptic neuron but
    //given a pointer to the post synaptic neuron to use when they are triggered.
    public void TriggerSynapes()
    {
        for(int s = 0; s < synList.Count; s++)
        {
            synList[s].Trigger();
        }
    }
    

}

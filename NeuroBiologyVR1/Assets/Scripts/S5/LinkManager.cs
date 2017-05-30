using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkManager : MonoBehaviour {

    private GameObject storedClick;
    private bool isStoredDendrite;
    public Color defColor, selectedColor, highlightColor;

    public VoltageUpdate n1_workaround, n2_workaround, n3_workaround;
    
    public void CreateTestSynapse(NeuronSlice n2_dendrite, ConveyerDS n2_junct)
    {
        n1_workaround.AddSynapse(new Synapse(30, 30, -65, n1_workaround.GetSlice(0), n2_dendrite, .002f, n2_junct));
    }

    public void CreateTestSynapse2(NeuronSlice n1_dendrite, ConveyerDS n1_junct)
    {
        n2_workaround.AddSynapse(new Synapse(30, 30, -65, n2_workaround.GetSlice(0), n1_dendrite, .002f, n1_junct));
    }

    public void CreateTestSynapse3(NeuronSlice n1_dendrite, ConveyerDS n1_junct)
    {
        n3_workaround.AddSynapse(new Synapse(30, 30, -65, n3_workaround.GetSlice(0), n1_dendrite, .002f, n1_junct));
    }

    public void HoverObject(bool isDendrite)    //if false, is an axon instead
    {
        Debug.Log("Mouse Enter");
        GameObject curObj = this.gameObject;
        if(storedClick == null || isStoredDendrite ^ isDendrite)
        {
            //highlight the object
            curObj.GetComponent<MeshRenderer>().GetComponent<Material>().color = highlightColor;
        }
        else
        {
            //no highlight change
        }
    }

    public void MouseLeave()
    {
        GameObject curObj = this.gameObject;
        if (storedClick == curObj)
        {
            //set color to select color
            curObj.GetComponent<MeshRenderer>().GetComponent<Material>().color = selectedColor;
        }
        else
        {
            //set color to default
            curObj.GetComponent<MeshRenderer>().GetComponent<Material>().color = defColor;
        }
    }

    public void ClickObject(bool isDendrite)    //if false, is an axon instead
    {
        GameObject curObj = this.gameObject;
        if (storedClick == null)
        {
            //highlight the object a selected color
            storedClick = curObj;
            isStoredDendrite = isDendrite;
            storedClick.GetComponent<MeshRenderer>().GetComponent<Material>().color = selectedColor;
        }
        else if ( isStoredDendrite ^ isDendrite)    //make the connection
        {
            VoltageUpdate storedVU = storedClick.GetComponentInParent<VoltageUpdate>();
            VoltageUpdate externVU = curObj.GetComponentInParent<VoltageUpdate>();
            Synapse newSyn = new Synapse();
            if (isDendrite)
            {
                for (int i = 0; i < externVU.denIndices.Length; i++)
                {
                    if (storedClick == externVU.GetObject(externVU.GetDenIndex(i)))
                    {
                        newSyn = new Synapse(30, 30, -65, externVU.GetSlice(externVU.GetDenIndex(i)), storedVU.GetSlice(0), .002f, externVU.rcVals);
                    }
                }
                externVU.AddSynapse(newSyn);
            }
            else
            {
                for(int i = 0; i < storedVU.denIndices.Length; i++)
                {
                    if(storedClick == storedVU.GetObject(storedVU.GetDenIndex(i)))
                    {
                        newSyn = new Synapse(30, 30, -65, storedVU.GetSlice(storedVU.GetDenIndex(i)), externVU.GetSlice(0), .002f, storedVU.rcVals);
                    }
                }
                storedVU.AddSynapse(newSyn);
            }
            storedClick.GetComponent<MeshRenderer>().GetComponent<Material>().color = defColor;
            storedClick = null;
        }
        else if (storedClick == curObj)
        {
            //deselect object
            storedClick = null;
            storedClick.GetComponent<MeshRenderer>().GetComponent<Material>().color = defColor;
        }
        else        //there is a stored object of the same type
        {
               //either nothing happens or selection changes
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DendriteSlice : NeuronSlice {

    private Synapse mySyn;
    public void AddSynapse(Synapse nSyn)
    {
        if(mySyn != null)
        {
            mySyn.Destroy();
        }
        mySyn = nSyn;
    }

    public void RemoveSynapse()
    {
        mySyn.Destroy();
        mySyn = null;
    }

}

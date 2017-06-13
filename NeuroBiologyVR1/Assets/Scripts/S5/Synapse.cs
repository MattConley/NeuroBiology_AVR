using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synapse {

    private float vp, kp, tauMax, tauSyn, gSynMax;

    private float[] dXVals = new float[2];
    public float x_1 { get; private set; }
    public float x_2 { get; private set; }

    public bool isEnabled { get; private set; }
    public bool isSimple { get; private set; }

    private float gateVolt, sendVolt, restVolt, revPotential;
    private NeuronSlice preSyn, postSyn, junct;
    private float denLen;
    private SummingJunction rcJunct;

    public Synapse() { }

    //This constructor is to be used with a non Srinivasan-Chiel synapse
    public Synapse(float tMax, float vP_i, float kP_i, float tSyn, float gSyn, float x1_i, float x2_i)
    {
        isSimple = false;
        isEnabled = true;
        tauMax = tMax;
        vp = vP_i;
        kp = kP_i;
        gSynMax = gSyn;
        tauSyn = tSyn;
        x_1 = x1_i;
        x_2 = x2_i;
    }

    //The synapse has references to the pre and post synaptic cells
    public Synapse(float activeVoltage, float sendVoltage, float restVoltage, NeuronSlice sender, NeuronSlice receiver, float dLen, SummingJunction junct, float eSyn)
    {
        //need length of dendrite
        isSimple = true;
        gateVolt = activeVoltage;
        sendVolt = sendVoltage;
        preSyn = sender;
        postSyn = receiver;
        restVolt = restVoltage;
        denLen = dLen;
        rcJunct = junct;
        revPotential = eSyn;
    }

    //Lets the summing junction know that the synapse has fired, then calculates Gsyn
    //and sets the voltage at the rc Cell based thereon
    public float Trigger()
    {
        rcJunct.Stimulate();
        float g_val = rcJunct.GetG();
        float newVolt = g_val * (revPotential - rcJunct.GetVoltage());
        rcJunct.SetVoltage(newVolt + rcJunct.GetVoltage());

        return sendVolt;
    }

    public float TransmittedVoltage(float axonVoltage)
    {
        float tranV = 0f;
        tranV = (tauMax) / (1 + Mathf.Exp(-(axonVoltage - vp)/kp));
        return tranV;
    }

    public float[] CalcDifferentials(float inVOlt)
    {
        dXVals[0] = TransmittedVoltage(inVOlt) - (2 / tauSyn) * x_2 - (1 / (tauSyn * tauSyn)) * x_1;
        dXVals[1] = x_1;
        return dXVals;
    }

    public void updateDVals(float dT)
    {
        x_1 += dXVals[0] * dT;
        x_2 += dXVals[1] * dT;
    }

    public void setEnabled(bool en)
    {
        isEnabled = en;
    }
}

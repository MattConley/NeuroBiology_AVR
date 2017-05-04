using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synapse : MonoBehaviour {

    private float vp, kp, tauMax, tauSyn, gSynMax;

    private float[] dXVals = new float[2];
    public float x_1 { get; private set; }
    public float x_2 { get; private set; }

    public bool isEnabled { get; private set; }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Synapse(float tMax, float vP_i, float kP_i, float tSyn, float gSyn, float x1_i, float x2_i)
    {
        isEnabled = true;
        tauMax = tMax;
        vp = vP_i;
        kp = kP_i;
        gSynMax = gSyn;
        tauSyn = tSyn;
        x_1 = x1_i;
        x_2 = x2_i;
    }

    public float TransmittedVoltage(float axonVoltage)
    {
        float tranV = 0f;
        tranV = (tauMax) / (1 + Mathf.Exp(-(axonVoltage - vp)/kp));
        return tranV;
    }

    public float[] CalcDifferentials(float inVOlt)
    {
        dXVals = new float[2];
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

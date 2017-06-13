using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummingJunction{
    private float sum1, sum2, tau, gSyn, currentV, deltaT, totalT;
    private bool isStim;

    public SummingJunction(float s1, float s2, float t, float restV, float delT)
    {
        sum1 = s1;
        sum2 = s2;
        tau = t;
        currentV = restV;
        deltaT = delT;
        totalT = 0;
        gSyn = 0;
        isStim = false; 
    }

    public void UpdateSums()
    {
        int s_val = isStim ? 1 : 0;
        sum1 = Mathf.Exp(-1f * deltaT / tau) * sum1 + s_val;
        sum2 = Mathf.Exp(-1f * deltaT / tau) * sum2 + s_val*totalT;

        gSyn = (1 / tau) * ((totalT + deltaT) * sum1 - sum2);
        totalT += deltaT;
        /*if (isStim)
        {
            Debug.Log(sum1);
            Debug.Log(sum2);
            Debug.Log("//\n");
        }/**/
        isStim = false;

        

    }

    public float GetG()
    {
        return gSyn;
    }

    public void Stimulate()
    {
        isStim = true;
    }

    public void SetVoltage(float newVolt)
    {
        currentV = newVolt;
    }
    public float GetVoltage()
    {
        return currentV;
    }
}

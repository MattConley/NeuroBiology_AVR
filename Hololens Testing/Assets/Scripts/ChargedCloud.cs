using UnityEngine;
using System.Collections;

public class ChargedCloud : MonoBehaviour {
    public static int chargeCloud = 0;
    GameObject Membrane;
    void Start()
    {
        UpdateColorCloud(0);
    }
    public static void UpdateColorCloud(int addedValue)
    {
        chargeCloud += addedValue;
        Color color = Color.clear;
        if (chargeCloud > 0)
            color = Color.red;
        else if (chargeCloud < 0)
            color = Color.blue;
        else
            color = Color.gray;
        FindObjectOfType<Renderer>().material.color = color;
    }

}
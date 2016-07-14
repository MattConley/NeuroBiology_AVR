using UnityEngine;
using System.Collections;

public class ChargedParticle : MonoBehaviour {
    public float charge = 1;
    void Start(){
        UpdateColor();
    }
    public void UpdateColor(){
        Color color = charge > 0 ? Color.red : Color.blue;
        GetComponent<Renderer>().material.color = color;
    }
}

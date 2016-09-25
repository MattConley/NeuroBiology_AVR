using UnityEngine;
using System.Collections;

public class ChangeColor : MonoBehaviour {
    public Renderer band;
    public int rValue;
    public int gValue;
    public int bValue;

    void Start()
    {
      Color bandColor = new Color(rValue, gValue, bValue, .75f);
        band.material.color = bandColor;

    }
  
}

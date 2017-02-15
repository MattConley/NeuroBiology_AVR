using UnityEngine;
using System.Collections;
using System;
public class Zoom_Behavior : MonoBehaviour {

    //public ParticleSystem part_pipette;
    private bool isEnabled;
    public int scene;

    // Use this for initialization
    public void Start () {
        isEnabled = true;
	}


    public void OnMouseDown()
    {
        if (isEnabled)
        {
            LoadScene(scene);
        }
    }

    public void LoadScene(int level)
    {
        
        Application.LoadLevel(level);
        ScoreTracker.Reset(0);
    }

}

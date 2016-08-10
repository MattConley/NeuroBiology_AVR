using UnityEngine;
using System.Collections;

public class Zoom_Behavior : MonoBehaviour {

    public ParticleSystem part_pipette;
    private bool isEnabled;


    // Use this for initialization
    void Start () {
        isEnabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        if (isEnabled)
        {
            LoadScene(2);
        }
    }

    public void LoadScene(int level)
    {
        Application.LoadLevel(level);
    }
}

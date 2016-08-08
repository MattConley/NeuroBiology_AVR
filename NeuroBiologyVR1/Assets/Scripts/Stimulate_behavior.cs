using UnityEngine;
using System.Collections;

public class Stimulate_behavior : MonoBehaviour {

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
            ParticleSystem pSystem = (ParticleSystem)Instantiate(part_pipette, part_pipette.transform.position, part_pipette.transform.rotation);
            pSystem.startDelay = 0;
            //pSystem = 2.0F;
            //isEnabled = false;
        }
    }
}

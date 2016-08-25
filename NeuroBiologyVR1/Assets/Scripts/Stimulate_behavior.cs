using UnityEngine;
using System.Collections;

public class Stimulate_behavior : MonoBehaviour {

    public ParticleSystem part_pipette;
    private bool isEnabled;

    //private bool isLooking;   For use with alternate strategy


    // Use this for initialization
    void Start () {
        isEnabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /*Alternate strategy
    void LateUpdate()
    {
        GvrViewer.Instance.UpdateState();
        if (GvrViewer.Instance.Triggered)
        {

        }
    }*/

     
    void OnTrigger()
    {
        ParticleSystem pSystem = (ParticleSystem)Instantiate(part_pipette, part_pipette.transform.position, part_pipette.transform.rotation);
        pSystem.startDelay = 0;
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

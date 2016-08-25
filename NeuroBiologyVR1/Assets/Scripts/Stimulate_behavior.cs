using UnityEngine;
using System.Collections;

public class Stimulate_behavior : MonoBehaviour {

    public ParticleSystem part_pipette;
    private bool isEnabled;

    //private bool isLooking;   For use with alternate strategy


    // Use this for initialization
    public void Start () {
        isEnabled = true;
	}
	

    /*Alternate strategy
    void LateUpdate()
    {
        GvrViewer.Instance.UpdateState();
        if (GvrViewer.Instance.Triggered)
        {

        }
    }*/

     
    public void OnTrigger()
    {
        ParticleSystem pSystem = (ParticleSystem)Instantiate(part_pipette, part_pipette.transform.position, part_pipette.transform.rotation);
        pSystem.startDelay = 0;
    }

    public void OnMouseDown()
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

using UnityEngine;
using System.Collections;


//TODO: Move click script from text to external manager, and call method within text


public class Prompt_Behavior : MonoBehaviour {
    

    //private int clickStatus = 0;
    public bool isContinue;
    public float contTime;

    private bool notDone = true;

    //public ParticleSystem part_pipette;

	// Use this for initialization
	void Start () {
       
        if (isContinue)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            //StartCoroutine(TimeEnable());
        }
        else
        {
            contTime = 0.0F;
            notDone = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (notDone&&isContinue)
        {
            if(Time.realtimeSinceStartup > contTime)
            {
                this.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }
    
}


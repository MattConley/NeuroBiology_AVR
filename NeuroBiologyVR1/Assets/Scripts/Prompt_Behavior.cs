using UnityEngine;
using System.Collections;


//TODO: Move click script from text to external manager, and call method within text


public class Prompt_Behavior : MonoBehaviour {

    public Camera player_cam;

    private int clickStatus = 0;
    public int cCounter = 0;

    public ParticleSystem part_pipette;

	// Use this for initialization
	void Start () {
        this.transform.LookAt(player_cam.transform);
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.LookAt(player_cam.transform);
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(mouseClick());
        }
    }

    IEnumerator mouseClick()
    {
        cCounter++;
        switch (clickStatus)
        {
            case 0:
                //first click
                this.GetComponent<MeshRenderer>().enabled = false;
                //part_pipette.startDelay = 0;
                ParticleSystem pSystem = (ParticleSystem)Instantiate(part_pipette, part_pipette.transform.position, part_pipette.transform.rotation);
                pSystem.startDelay = 0;
                //wait for animation
                yield return new WaitForSeconds(2);
                this.GetComponent<MeshRenderer>().enabled = true;
                this.GetComponent<TextMesh>().text = "Click to Zoom";
                clickStatus++;
                break;
            case 1:
                //Second Click
                this.GetComponent<MeshRenderer>().enabled = false;
                //Scene Change
                yield return null;
                break;
        }
    }


}

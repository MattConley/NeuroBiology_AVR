using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;


public class HTK_Interactive : MonoBehaviour, IInputHandler, IFocusable {

    public enum InteractableFunction
    {
        graphToggle, modeToggle, pauseToggle,
        diameterMinus, diameterPlus, graphRescale, 
        applicationExit, particleToggle, voiceToggle
    }
    public ScriptManager myMan;
    /*public bool graphToggle, modeToggle, pauseToggle, 
        diameterSmall, diameterMed, diameterLarge;*/

    public InteractableFunction myFunction;
    public void OnInputDown(InputEventData eventData)
    {
        Debug.Log("Triggered");
        switch (myFunction)
        {
            case InteractableFunction.graphToggle:
                this.GetComponent<MeshRenderer>().enabled = false;      //with two toggles, the gaze doesn't leave
                myMan.ToggleGraph();
                break;
            case InteractableFunction.modeToggle:
                myMan.ToggleMode();
                break;
            case InteractableFunction.pauseToggle:
                myMan.TogglePause();
                break;
            case InteractableFunction.diameterMinus:
                this.GetComponent<MeshRenderer>().enabled = false;      //same issue as toggle, sometimes
                myMan.SetDiameter(-1);
                break;
            case InteractableFunction.diameterPlus:
                this.GetComponent<MeshRenderer>().enabled = false;
                myMan.SetDiameter(1);
                break;
            case InteractableFunction.graphRescale:
                myMan.RescaleGraph();
                break;
            case InteractableFunction.applicationExit:
                myMan.ExitApplication();
                break;
            case InteractableFunction.particleToggle:
                myMan.ToggleParticles();
                break;
            case InteractableFunction.voiceToggle:
                myMan.ToggleVoice();
                break;
            default:
                break;
        }
    }

    public void OnInputUp(InputEventData eventData)
    {
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnFocusEnter()
    {
        this.GetComponent<MeshRenderer>().enabled = true;
    }

    public void OnFocusExit()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
    }
}

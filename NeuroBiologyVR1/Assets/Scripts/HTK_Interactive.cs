using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;


public class HTK_Interactive : MonoBehaviour, IInputHandler, IFocusable {

    public enum InteractableFunction
    {
        graphToggle, modeToggle, pauseToggle,
        diameterMinus, diameterPlus, graphRescale
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
                myMan.ToggleGraph();
                break;
            case InteractableFunction.modeToggle:
                myMan.ToggleMode();
                break;
            case InteractableFunction.pauseToggle:
                myMan.TogglePause();
                break;
            case InteractableFunction.diameterMinus:
                myMan.SetDiameter(-1);
                break;
            case InteractableFunction.diameterPlus:
                myMan.SetDiameter(1);
                break;
            case InteractableFunction.graphRescale:
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
        //enable mesh renderer
    }

    public void OnFocusExit()
    {
        //disable mesh renderer
    }
}

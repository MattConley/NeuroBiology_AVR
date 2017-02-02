using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LabelButtonBehavior : MonoBehaviour {

    //
    public GameObject emptyMan;
    public ScriptManager myManager;
	// Use this for initialization
	void Start () {
        myManager = emptyMan.GetComponent<ScriptManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ToggleGraph()
    {
        myManager.ToggleGraph();
    }

    public void PauseTime()
    {
        bool isPaused = myManager.TogglePause();
        if (isPaused)
        {
            this.GetComponentInChildren<Text>().text = "Unpause";
        }
        else
        {
            this.GetComponentInChildren<Text>().text = "Pause";
        }
    }
}

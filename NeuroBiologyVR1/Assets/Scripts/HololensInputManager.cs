using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;


public class HololensInputManager : MonoBehaviour {

    private GestureRecognizer holo_gesture;
    public GameObject player_obj;
    public Camera player_cam;
    public GameObject man_obj;
    private ScriptManager myManager;
    private RaycastHit gaze_ray = new RaycastHit();

	// Use this for initialization
	void Start () {
        holo_gesture = new GestureRecognizer();
        holo_gesture.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CalcGaze()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(player_cam.transform.position, player_cam.transform.forward, out hitInfo, Mathf.Infinity, -10))
        {
            gaze_ray = hitInfo;

        }
    }

    public RaycastHit GetGaze()
    {
        CalcGaze();         //could be called somewhere else
        return gaze_ray;
    }

    private void OnDestroy()
    {
        holo_gesture.TappedEvent += null;
        holo_gesture.HoldCanceledEvent += null;
        holo_gesture.HoldCompletedEvent += null;
        holo_gesture.HoldStartedEvent += null;
    }

    public void ChangeTappedEvent(bool isAdd, GestureRecognizer.TappedEventDelegate method)
    {
        if (isAdd)
            holo_gesture.TappedEvent += method;
        else
            holo_gesture.TappedEvent -= method;
    }

    public void ChangeHoldStart(bool isAdd, GestureRecognizer.HoldStartedEventDelegate method)
    {
        if (isAdd)
            holo_gesture.HoldStartedEvent += method;
        else
            holo_gesture.HoldStartedEvent -= method;
    }

    public void ChangeHoldCancel(bool isAdd, GestureRecognizer.HoldCanceledEventDelegate method)
    {
        if (isAdd)
            holo_gesture.HoldCanceledEvent += method;
        else
            holo_gesture.HoldCanceledEvent -= method;
    }

    public void ChangeHoldComplete(bool isAdd, GestureRecognizer.HoldCompletedEventDelegate method)
    {
        if (isAdd)
            holo_gesture.HoldCompletedEvent += method;
        else
            holo_gesture.HoldCompletedEvent -= method;
    }
}

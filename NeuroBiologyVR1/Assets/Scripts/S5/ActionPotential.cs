using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPotential {//: MonoBehaviour {

    public GameObject pulseRing;
    private float remainingTime, myVelocity;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public ActionPotential(GameObject neuronParent, GameObject pulseObj, Vector3 init_pos, Quaternion direction, float velocity, float axonDist)
    {
        pulseRing =  Object.Instantiate(pulseObj, init_pos, direction, neuronParent.transform);
        myVelocity = velocity;
        remainingTime = axonDist / velocity;    //Assuming units are constant
    }

    public bool ShouldDespawn(float passedTime)
    {
        if (remainingTime <= 0f)
            return true;
        //update position
        pulseRing.transform.localPosition = pulseRing.transform.localPosition + new Vector3(myVelocity * passedTime, 0);    //assuming it travels in the positive x direction (along the axon)
        //update remainingTime
        remainingTime -= passedTime;
        return false;
    }

    public void Destroy()
    {
        Object.Destroy(pulseRing);
    }



}

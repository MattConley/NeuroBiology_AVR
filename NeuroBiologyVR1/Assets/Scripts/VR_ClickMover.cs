using UnityEngine;
using System.Collections;

public class VR_ClickMover : MonoBehaviour {

    public GameObject gvr_viewer; 

    private Transform player_body;
    private Transform player_viewer;
    private Vector3 cam_direction;
    private int moveLen = 5;

	// Use this for initialization
	void Start () {
        player_body = this.GetComponent<Transform>();
        player_viewer = gvr_viewer.GetComponent<Transform>();
        cam_direction = player_body.forward;
	}
	
	// Update is called once per frame
	void Update () {
        //cam_direction = player_body.forward;
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            cam_direction = player_viewer.forward;
            //Debug.Log("" + cam_direction * moveLen);
            player_body.position = player_body.position + cam_direction * moveLen;
        }
#if (UNITY_IPHONE)
        if (GvrController.ClickButton)
        {
            player_body.position = cam_direction * moveLen;
        }
#endif
    }
}

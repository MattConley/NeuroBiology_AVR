using UnityEngine;
using System.Collections;

public class VR_ClickMover : MonoBehaviour {

    public GameObject gvr_viewer; 

    private Transform player_transform;
    private Transform player_viewer;
    private Rigidbody player_rbody;
    private Vector3 cam_direction;
    private int moveLen = 5;
    private int moveSpeed = 100;
    private float slowRate = (float)1.5;

	// Use this for initialization
	void Start () {
        player_transform = this.GetComponent<Transform>();
        player_rbody = this.GetComponent<Rigidbody>();
        player_viewer = gvr_viewer.GetComponent<Transform>();
        cam_direction = player_transform.forward;
        player_rbody.velocity = new Vector3(0, 0, 0);
        Debug.Log("" + cam_direction);
        player_rbody.useGravity = false;

    }
	
	// Update is called once per frame
	void Update () {
        //cam_direction = player_body.forward;
        //Debug.Log("" + cam_direction);
        if (Input.GetKey(KeyCode.W))
        {
            cam_direction = player_viewer.forward;
            //player_transform.position = player_transform.position + cam_direction * moveLen;
            player_rbody.velocity = new Vector3(cam_direction.x * moveSpeed, cam_direction.y * moveSpeed, cam_direction.z * moveSpeed);
            //player_rbody.AddForce(cam_direction.x * moveSpeed, cam_direction.y * moveSpeed, cam_direction.z * moveSpeed);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            cam_direction = new Vector3(player_viewer.right.x*-1, player_viewer.right.y * -1, player_viewer.right.z * -1);
            player_rbody.velocity = new Vector3(cam_direction.x * moveSpeed, cam_direction.y * moveSpeed, cam_direction.z * moveSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            cam_direction = player_viewer.right;
            player_rbody.velocity = new Vector3(cam_direction.x * moveSpeed, cam_direction.y * moveSpeed, cam_direction.z * moveSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            cam_direction = new Vector3(player_viewer.forward.x * -1, player_viewer.forward.y * -1, player_viewer.forward.z * -1);
            player_rbody.velocity = new Vector3(cam_direction.x * moveSpeed, cam_direction.y * moveSpeed, cam_direction.z * moveSpeed);
        }
        else
        {
            player_rbody.velocity = new Vector3(0,0,0);
        }
#if (UNITY_IPHONE)
        if (GvrController.ClickButton)
        {
            player_body.position = cam_direction * moveLen;
        }
#endif
    }
}

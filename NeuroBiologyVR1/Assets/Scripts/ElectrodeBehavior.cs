﻿using UnityEngine;
//using UnityEngine.EventSystems;
using System.Collections;
using HoloToolkit.Unity.InputModule;

public class ElectrodeBehavior : MonoBehaviour, IInputHandler, IFocusable {

    public GameObject cube_manager;
    public GameObject empty_man;
    public Material high_mat;
    //public GameObject player_obj;
    public Camera player_cam;
    public GameObject gui_canvas;

    public float plane_dist;

    public Plane band_plane;
    //public GameObject band_plane;

    private Material def_mat;
    private Material band_def;

    private ChangeColor otherScript;
    private WMGTest graphScript;
    private ScriptManager myManager;
    public int last_band { get; set; }
    private int stim_pos = 20;      //position of stimulating electrode (zero indexed)

    private int max_band = 40;

    private float scale = 1;        //Used to update
    private float increment = 6;    //Base y_pos

    private float orig_ypos, y_pos, click_ypos, x_pos;
    private float tip_ypos, tip_zpos;

    private Vector3 pointer_pos;
    //private Vector3 vec_oldPos = new Vector3(3.7f, 119.6f, -147.5f);    //position Vector at x=30; //Before Scale Change

    private Vector3 vec_oldPos = new Vector3(3.7f, 4.85f, 23.75f);    //position Vector at x=30;

    private Ray holo_view;

    //public GameObject

    private bool isDragging = false;

    public bool isHolo;

	// Use this for initialization
	void Start () {
        if(isHolo)
            vec_oldPos = new Vector3(3.7f, 4.85f, 23.75f);
        else
            vec_oldPos = new Vector3(3.7f, 119.6f, -147.5f);

        last_band = 30;
        Vector3 tempVec = vec_oldPos;//this.GetComponent<Transform>().position;
        orig_ypos = tempVec.y;
        y_pos = tempVec.y;
        click_ypos = 0;
        x_pos = tempVec.x;
        def_mat = this.GetComponent<MeshRenderer>().materials[0];
        otherScript = cube_manager.GetComponent<ChangeColor>();
        myManager = empty_man.GetComponent<ScriptManager>();
        //otherScript.bandMat_high = high_mat;

        graphScript = gui_canvas.GetComponent<WMGTest>();

        band_plane = new Plane(new Vector3(0, 0, 0), plane_dist);
        
    }
	
    private void updateBand(float z_pos)
    {
        int new_band = (int)(-1 * (z_pos / 5)) + 1;
        if(new_band == stim_pos || new_band == last_band)
        {
            return;
        }
        else if(new_band < 0 )
        {
            otherScript.ResetBand(last_band);
            last_band = -1;
            //otherScript.HighlightBand(0, last_band);
            //last_band = 0;
        }
        else if(new_band > max_band)
        {
            otherScript.ResetBand(last_band);
            last_band = -1;
            //otherScript.HighlightBand(max_band, last_band);
            //last_band = max_band;
        }
        else
        {
            otherScript.HighlightBand(new_band, last_band);
            last_band = new_band;
            //this.GetComponent<Transform>().position = new Vector3();
        }
    }

    public void UpdateYPos(float value)
    {
        y_pos = orig_ypos + (value - 1) * increment;
    }

	// Update is called once per frame
	void Update () {
        if (isDragging)
        {
            float cam_z_depth = Mathf.Abs(player_cam.GetComponent<Transform>().position.x - x_pos);

            
            if (isHolo)
            {
                holo_view = new Ray(player_cam.transform.position, player_cam.transform.forward);
                float dist;
                band_plane.Raycast(holo_view, out dist);
                pointer_pos = holo_view.GetPoint(dist);
            }
            else
            {
                pointer_pos = player_cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam_z_depth));
            }

            //float y_intersect = 0f, z_intersect = 0f;
            //calc camera-pointer ray intersection with cable x-y plane

            //set electrode position based on mouse/pointer
            this.GetComponent<Transform>().position = new Vector3(x_pos, pointer_pos.y + click_ypos, pointer_pos.z);

            if (this.GetComponent<Transform>().position.y - 120 <= 0)
            {
                updateBand(pointer_pos.z);
            }
            else
            {
                otherScript.ResetBand(last_band);
                last_band = -1;
            }
        }
    }

    public void Mouse_Hover(bool isEnter)
    {
        if (isEnter)
        {
            this.GetComponent<MeshRenderer>().material = high_mat;
        }
        else if(!isDragging)
        {
            this.GetComponent<MeshRenderer>().material = def_mat;
        }
    }

    public void OnInputDown(InputEventData hold_data)
    {
        Debug.Log("Begun");
        onDrag(true);
    }

    public void OnInputUp(InputEventData hold_data)
    {
        Debug.Log("Can");
        onDrag(false);
    }

    public void OnHoldCompleted(HoldEventData hold_data)
    {
        Debug.Log("COmp");
        onDrag(false);
    }

    public void OnFocusEnter()
    {
        Debug.Log("Enter");
    }

    public void OnFocusExit()
    {
        Debug.Log("Exit");
    }

    public void onDrag(bool isBegun)
    {
        isDragging = isBegun;
        if (isBegun)
        {
            //disable graph series 2
            //graphScript.set_recEnabled(false);
            myManager.DisableElectrode();
            
            if (isHolo)
            {
                holo_view = new Ray(player_cam.transform.position, player_cam.transform.forward);
                float dist;
                band_plane.Raycast(holo_view, out dist);
                pointer_pos = holo_view.GetPoint(dist);
            }
            else
            {
                pointer_pos = player_cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(player_cam.GetComponent<Transform>().position.x - x_pos)));
            }

            Debug.Log(pointer_pos);

            click_ypos = this.GetComponent<Transform>().position.y - pointer_pos.y;
        }
        else
        {
            if (last_band >= 0)
            {
                myManager.UpdateElectrode(last_band);
                //update transform position
                this.GetComponent<Transform>().position = new Vector3(x_pos, y_pos, -1f * (float)((last_band - 1) * 5 + 2.5));
                //reset target_band's material
                //otherScript.ResetBand(last_band);
                //enable graph series 2
                //graphScript.set_recEnabled(true);
            }
            else
            {
                myManager.DisableElectrode();
            }
        }
    }
}

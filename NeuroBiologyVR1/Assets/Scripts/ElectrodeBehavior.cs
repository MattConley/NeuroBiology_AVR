using UnityEngine;
//using UnityEngine.EventSystems;
using System.Collections;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA.Input;
using System;

public class ElectrodeBehavior : MonoBehaviour, IFocusable {

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

    private Vector3 old_manip = new Vector3(), current_manip;

    private Vector3 pre_Pos = new Vector3();

    private Ray holo_view;

    //public GameObject

    private bool isDragging = false, isFocused = false;

    public bool isHolo, notToolkit;

    private GestureRecognizer gesture_rec;

	// Use this for initialization
	void Start () {
        /*
        if (isHolo)
            vec_oldPos = new Vector3(3.7f, 4.85f, 23.75f);
        else
            vec_oldPos = new Vector3(3.7f, 119.6f, -147.5f);
        */
        vec_oldPos = new Vector3(3.7f, 119.6f, -147.5f);    //localPos uses this value
        /**/
        gesture_rec = new GestureRecognizer();
        gesture_rec.SetRecognizableGestures(GestureSettings.ManipulationTranslate);
        gesture_rec.ManipulationStartedEvent += Manipulation_Started;
        gesture_rec.ManipulationCompletedEvent += Manipulation_Finished;
        gesture_rec.ManipulationCanceledEvent += Manipulation_Cancelled;
        gesture_rec.ManipulationUpdatedEvent += Manipulation_Updated;
        gesture_rec.StartCapturingGestures();
        /**/

        last_band = 30;
        Vector3 tempVec = vec_oldPos;//this.GetComponent<Transform>().position;
        pre_Pos = this.GetComponent<Transform>().localPosition;
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

    /**/
    private void OnDestroy()
    {
        gesture_rec.StopCapturingGestures();
        gesture_rec.ManipulationStartedEvent -= Manipulation_Started;
        gesture_rec.ManipulationCompletedEvent -= Manipulation_Finished;
        gesture_rec.ManipulationCanceledEvent -= Manipulation_Cancelled;
        gesture_rec.ManipulationUpdatedEvent -= Manipulation_Updated;
    }/**/
    /**/
    private void Manipulation_Updated(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        current_manip = cumulativeDelta;
        //throw new System.NotImplementedException();
    }

    private void Manipulation_Cancelled(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        Debug.Log("Manipulation Cancelled");
        current_manip = cumulativeDelta;
        onDrag(false);
        //throw new System.NotImplementedException();
    }

    private void Manipulation_Finished(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        Debug.Log("Manipulation Finished");
        current_manip = cumulativeDelta;
        onDrag(false);
        //throw new System.NotImplementedException();
    }

    private void Manipulation_Started(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        if (!isFocused)
            return;
        Debug.Log("MANIPULATION Started");
        current_manip = cumulativeDelta;
        onDrag(true);
        //throw new System.NotImplementedException();
    }/**/
    
    public void TransformAdjust(float val)
    {
        if (val == scale)
            return;
        else if (val < scale)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - (scale - val) * increment, transform.localPosition.z);
        else
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + (val - scale) * increment, transform.localPosition.z);
        scale = val;

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

    public LayerMask raycastLayer;

	// Update is called once per frame
	void Update () {
        /*GESTURES EXAMPLE
        GestureRecognizer gr = new GestureRecognizer();
        gr.ManipulationStartedEvent += Gr_ManipulationStartedEvent;
        gr.SetRecognizableGestures(GestureSettings.ManipulationTranslate);
        gr.StartCapturingGestures();
        */


        if (isDragging)
        {

            /*RaycastHit hit;
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10f, raycastLayer)){
                hit.point
            }*/


            float cam_z_depth = Mathf.Abs(player_cam.GetComponent<Transform>().position.x - x_pos);

            if (isHolo && notToolkit)
            {
                float manx, many, manz;     //need to be broken down because of 90 deg Y rotation
                manx = current_manip.x;
                many = current_manip.y;
                manz = current_manip.z;

                this.GetComponent<Transform>().localPosition += new Vector3(5*manz, 10*many, -5*manx);

                //this.GetComponent<Transform>().localPosition += (5*current_manip);
                //this.GetComponent<Transform>().position += current_manip;
                //Debug.Log(current_manip);
            }
            else if (isHolo)
            {
                holo_view = new Ray(player_cam.transform.position, player_cam.transform.forward);
                float dist;
                band_plane.Raycast(holo_view, out dist);
                pointer_pos = holo_view.GetPoint(dist);

                RaycastHit gaze_hit;

                if(Physics.Raycast(player_cam.transform.position, player_cam.transform.forward, out gaze_hit, Mathf.Infinity, raycastLayer))
                {
                    pointer_pos = gaze_hit.point;
                }
                else
                {
                    onDrag(false);
                    return;
                }
                this.GetComponent<Transform>().localPosition = new Vector3(x_pos, pointer_pos.y + click_ypos, pointer_pos.z);
            }
            else
            {
                pointer_pos = player_cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam_z_depth));
                this.GetComponent<Transform>().localPosition = new Vector3(x_pos, pointer_pos.y + click_ypos, pointer_pos.z);
            }

            //Debug.Log(pointer_pos);

            //float y_intersect = 0f, z_intersect = 0f;
            //calc camera-pointer ray intersection with cable x-y plane

            //set electrode position based on mouse/pointer
            //this.GetComponent<Transform>().position = new Vector3(x_pos, pointer_pos.y + click_ypos, pointer_pos.z);
            
            

            if (this.GetComponent<Transform>().localPosition.y - 120 <= 0)
            {
                //updateBand(pointer_pos.z);
                updateBand(this.transform.localPosition.z);
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
        isFocused = isEnter;
        if (isEnter)
        {
            this.GetComponent<MeshRenderer>().material = high_mat;
        }
        else if(!isDragging)
        {
            this.GetComponent<MeshRenderer>().material = def_mat;
        }
    }

    public void OnFocusEnter()
    {
        Debug.Log("Enter");
        Mouse_Hover(true);
    }

    public void OnFocusExit()
    {
        Debug.Log("Exit");
        Mouse_Hover(false);
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

                RaycastHit gaze_hit;

                if (Physics.Raycast(player_cam.transform.position, player_cam.transform.forward, out gaze_hit, Mathf.Infinity, raycastLayer))
                {
                    pointer_pos = gaze_hit.point;
                }
            }
            else
            {
                pointer_pos = player_cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(player_cam.GetComponent<Transform>().position.x - x_pos)));
            }

            Debug.Log(pointer_pos);

            //click_ypos = this.GetComponent<Transform>().position.y - pointer_pos.y;
            click_ypos = this.GetComponent<Transform>().localPosition.y - pointer_pos.y;
        }
        else
        {
            if (myManager.passedTransform)      //electrode has a new location
            {
                //do nothing, updated from manager
                //actually, update last band then updateElectrode
                return;
            }
            else
            {
                if (last_band >= 0)
                {
                    myManager.UpdateElectrode(last_band);
                    //update transform position
                    //this.GetComponent<Transform>().position = new Vector3(x_pos, y_pos, -1f * (float)((last_band - 1) * 5 + 2.5));
                    this.GetComponent<Transform>().localPosition = new Vector3(x_pos, y_pos, -1f * (float)((last_band - 1) * 5 + 2.5));
                    //reset target_band's material
                    //otherScript.ResetBand(last_band);
                    //enable graph series 2
                    //graphScript.set_recEnabled(true);
                }
                else
                {
                    myManager.DisableElectrode();
                    float temp_ypos = this.transform.localPosition.y, temp_zpos = this.transform.localPosition.z;
                    if (temp_ypos < 0 || temp_ypos > 275)
                        temp_ypos = pre_Pos.y;
                    if (temp_zpos < -250 || temp_zpos > 50)
                        temp_zpos = pre_Pos.z;
                    this.transform.localPosition = new Vector3(x_pos, temp_ypos, temp_zpos);
                }

                //electrode position needs to be sent out
                myManager.SendUpdatedElectrode(this.transform.localPosition);
            }
        }
            
    }

    public void SetLocalPos(Vector3 newLocPos)
    {
        this.GetComponent<Transform>().localPosition = newLocPos;
    }
    
}

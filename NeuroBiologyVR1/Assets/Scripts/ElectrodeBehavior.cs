using UnityEngine;
//using UnityEngine.EventSystems;
using System.Collections;

public class ElectrodeBehavior : MonoBehaviour {

    public GameObject cube_manager;
    public Material high_mat;
    public GameObject player_obj;
    public Camera player_cam;
    public GameObject gui_canvas;

    private Material def_mat;
    private Material band_def;

    private ChangeColor otherScript;
    private WMGTest graphScript;
    public int last_band { get; set; }
    private int stim_pos = 20;      //position of stimulating electrode (zero indexed)

    private int max_band = 40;

    private float scale = 1;        //Used to update
    private float increment = 6;    //Base y_pos

    private float orig_ypos, y_pos, x_pos;

    //public GameObject

    private bool isDragging = false;

	// Use this for initialization
	void Start () {
        last_band = 30;
        Vector3 tempVec = this.GetComponent<Transform>().position;
        orig_ypos = tempVec.y;
        y_pos = tempVec.y;
        x_pos = tempVec.x;
        def_mat = this.GetComponent<MeshRenderer>().materials[0];
        otherScript = cube_manager.GetComponent<ChangeColor>();
        otherScript.bandMat_high = high_mat;

        graphScript = gui_canvas.GetComponent<WMGTest>();
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
            otherScript.HighlightBand(0, last_band);
            last_band = 0;
        }
        else if(new_band > max_band)
        {
            otherScript.HighlightBand(max_band, last_band);
            last_band = max_band;
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
            float cam_z_depth = Mathf.Abs(player_obj.GetComponent<Transform>().position.x - x_pos);
            Vector3 pointer_pos = player_cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam_z_depth));
            //float y_intersect = 0f, z_intersect = 0f;
            //calc camera-pointer ray intersection with cable x-y plane

            //set electrode position based on mouse/pointer
            this.GetComponent<Transform>().position = new Vector3(x_pos, pointer_pos.y, pointer_pos.z);

            updateBand(pointer_pos.z);
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

    public void onDrag(bool isBegun)
    {
        isDragging = isBegun;
        if (isBegun)
        {
            //disable graph series 2
            graphScript.set_recEnabled(false);
        }
        else
        {
            //update transform position
            this.GetComponent<Transform>().position = new Vector3(x_pos, y_pos, -1f * (float)(last_band * 5 + 2.5));
            //reset target_band's material
            otherScript.ResetBand(last_band);
            //enable graph series 2
            graphScript.set_recEnabled(true);
        }
    }
}

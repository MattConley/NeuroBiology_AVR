using UnityEngine;
using System.Collections;

public class ScriptManager : MonoBehaviour {

    public GameObject
        gui_canvas, rec_electrode, color_cube, player_obj, player_cam;

    public WMGTest graph_script;
    public ElectrodeBehavior rec_script;
    public ChangeColor color_script;

    public Material bandMat_high;
    
    public bool graph_enabled = false;
    private bool isPaused = false;

    public int band_width = 20;

    private float last_update;
    private double since_stimulation = 0.01;

    private double time_scale = 0.00002;         //seconds * time_scale = simulated seconds
    private double time_converter = 20;          //seconds * time_converter = simulated microseconds

    private int num_points = 55;

    public int e_pos = 30;

    // Use this for initialization
    void Start () {
        //get scripts from GameObjects
        graph_script = gui_canvas.GetComponent<WMGTest>();
        rec_script = rec_electrode.GetComponent<ElectrodeBehavior>();
        color_script = color_cube.GetComponent<ChangeColor>();

        color_script.bandMat_high = bandMat_high;
        //calculate voltage
        color_script.time_scale = time_scale;
        //init graph
        graph_script.num_points = num_points;
        graph_script.band_width = band_width;
        

	}
	
	// Update is called once per frame
	void Update () {
        if (isPaused)
            return;
        if (since_stimulation == 0.0)     //continuous stimulation framework
        {
            last_update = Time.deltaTime;
            //double[] voltage_data = color_script.UpdateVoltage(0.000001);
            //graph_script.AddPoint(voltage_data, (float)(last_update * time_converter));
            since_stimulation += Time.deltaTime;
        }
        else
        {
            last_update = Time.deltaTime;
            double total_time = since_stimulation + last_update;

            double[] voltage_data = color_script.UpdateVoltage(total_time * time_scale);

            graph_script.AddPoint(voltage_data, (float)(last_update*time_converter));
            since_stimulation = total_time;
        }
	}

    void SetVars()     //set variables to predetermined values
    {

    }

    public void ToggleGraph()
    {
        graph_script.ToggleGraph();
    }

    public bool TogglePause()       //returns whether the time is NOW paused
    {
        color_cube.SetActive(isPaused);
        if (!isPaused)
        {
            Time.timeScale = 0;
            graph_script.enabled = false;
            color_script.enabled = false;
            rec_script.enabled = false;
        }
        else
        {
            Time.timeScale = 1;
            graph_script.enabled = true;
            color_script.enabled = true;
            rec_script.enabled = true;
        }
        isPaused = !isPaused;
        return isPaused;
    }

    public void Stimulate()
    {
        since_stimulation = 0;
    }

    public void UpdateElectrode(int newEPos)
    {
        e_pos = newEPos;
        color_script.ResetBand(e_pos);
        graph_script.RecieveSliderValue(e_pos);
        graph_script.set_recEnabled(true);
    }
}

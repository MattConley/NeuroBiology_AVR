using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WMGTest : MonoBehaviour
{

    public GameObject emptyGraphPrefab;
    public GameObject graphGO;
    public GameObject CubeObject;
    public GameObject RecElectrode;
    public WMG_Axis_Graph graph;
    public WMG_Series series1;
    public WMG_Series series2;


    private ChangeColor otherScript;
    private ElectrodeBehavior rec_behavior;
    //private double [,] newMatTwo;
    private double[] newMatTwo;
    private int x = 20;
    private int t = 20;
    private int recordingSite = 10;

    public int band_width = 20;     //set by manager
    public int num_points = 200;

    private float coroutine_time;
    private float fupdate_time = 0.02f;
    private float update_count = 0f;

    public List<Vector2> data, data2, dataStore;

    private bool rec_enabled = true;

    public long curcount = 0; 

    void Start()
    {
        //graphGO = GameObject.Instantiate(emptyGraphPrefab);
        //graphGO.transform.SetParent(this.transform, false);

        graph = graphGO.GetComponent<WMG_Axis_Graph>();
        series1 = graph.addSeries();
        series2 = graph.addSeries();
        series2.pointColor = new Color(0, 230, 0);
        series2.lineColor = new Color(0, 230, 0);

        graph.xAxis.AxisMinValue = 20;
        graph.xAxis.AxisMaxValue = 0;
        graph.autoUpdateOrigin = false;
        graph.theOrigin = new Vector2(0f, 0f);
        graph.xAxis.AxisTitleString = "Time";
        graph.xAxis.hideLabels = true;
        graph.autoShrinkAtPercent = 0f;
        graph.autoAnimationsEnabled = false;

        coroutine_time = 0.15f;//otherScript.coroutine_wait;

        otherScript = CubeObject.GetComponent<ChangeColor>();
        rec_behavior = RecElectrode.GetComponent<ElectrodeBehavior>();

        data = new List<Vector2>();
        data2 = new List<Vector2>();
        dataStore = new List<Vector2>();

        //series1.pointValues
        //graph.xAxis.AxisMaxValue = x;

    }


    public void RecieveSliderValue(float valuez)
    {
        recordingSite = (int)valuez;
    }

    public void set_recEnabled(bool ren)
    {
        rec_enabled = ren;
        series2.enabled = ren;
        //dataStore = data2;
        //data2 = new List<Vector2>();
    }

    public void ToggleGraph()
    {
        graphGO.SetActive(!graphGO.activeSelf);
    }

    // Use this for initialization
    void nUpdate()
    {
        curcount++;
        if (update_count >= coroutine_time)
        {
            update_count = update_count % coroutine_time;


            //otherScript.UpdateRecorders();
            //recordingSite = otherScript.recordingSite;
            if (rec_enabled)
                recordingSite = rec_behavior.last_band;

            //newMatTwo = otherScript.newMat;
            newMatTwo = otherScript.cont_voltage;

            for (int i = 0; i < data.Count; i++)
            {
                float temp_x = data[i].x;
                data.Insert(i, new Vector2(temp_x + 1, data[i].y));
                data.RemoveAt(i + 1);
            }
            for (int i = 0; i < data2.Count; i++)
            {
                float temp_x = data2[i].x;
                data2.Insert(i, new Vector2(temp_x + 1, data2[i].y));
                data2.RemoveAt(i + 1);
            }
            /*
            for (int i = 0; i < dataStore.Count; i++)
            {
                float temp_x = dataStore[i].x;
                dataStore.Insert(i, new Vector2(temp_x + 1, dataStore[i].y));
                dataStore.RemoveAt(i + 1);
            }*/

            data.Insert(0, new Vector2(0f, (float)newMatTwo[20]));
            if (rec_enabled)
                data2.Insert(0, new Vector2(0f, (float)newMatTwo[recordingSite]));
            else
                data2.Insert(0, new Vector2(0f, 0f));

            /**/
            if (data.Count >= t)
                data.RemoveAt(t - 1);
            if (data2.Count >= t)
                data2.RemoveAt(t - 1);
            //if (dataStore.Count >= t)
            //    dataStore.RemoveAt(t - 1);
            /**/

            series1.pointValues.SetList(data);
            if(rec_enabled)
                series2.pointValues.SetList(data2);
            else
                series2.pointValues.SetList(new List<Vector2>());
            
            
        }
        else
        {
            update_count += fupdate_time;
        }
    }

    public void AddPoint(double[] volt, float timestep)
    {
        for(int i = 0; i < data.Count; i++)
        {
            Vector2 prevValue = data[i];
            data.Insert(i, new Vector2(prevValue.x + timestep, prevValue.y));
            data.RemoveAt(i + 1);

            prevValue = data2[i];
            data2.Insert(i, new Vector2(prevValue.x + timestep, prevValue.y));
            data2.RemoveAt(i + 1);
        }

        data.Insert(0, new Vector2(0f, (float)volt[band_width]));
        if (rec_enabled)
            data2.Insert(0, new Vector2(0f, (float)volt[recordingSite]));
        else
            data2.Insert(0, new Vector2(0f, 0f));

        if (data.Count >= num_points)
        {
            data.RemoveAt(num_points - 1);
            data2.RemoveAt(num_points - 1);
        }

        series1.pointValues.SetList(data);
        if (rec_enabled)
            series2.pointValues.SetList(data2);
        else
            series2.pointValues.SetList(new List<Vector2>());

    }

}

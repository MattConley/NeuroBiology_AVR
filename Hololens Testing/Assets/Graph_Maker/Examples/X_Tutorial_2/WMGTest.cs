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
    public int pointBuffer;

    private float coroutine_time;
    private float fupdate_time = 0.02f;
    private float update_count = 0f;

    public List<Vector2> data, data2, dataStore, data2Store;

    private bool rec_enabled = false;
    private bool graph_enabled = false;
    private bool analysis_mode = false;

    private Vector3 cur_pos;
    private Vector3 alt_pos = new Vector3(0, -190, -100);

    public long curcount = 0; 

    void Start()
    {
        //graphGO = GameObject.Instantiate(emptyGraphPrefab);
        //graphGO.transform.SetParent(this.transform, false);
        //graphGO.SetActive(graph_enabled);

        graph = graphGO.GetComponent<WMG_Axis_Graph>();
        series1 = graph.addSeries();
        series2 = graph.addSeries();
        series2.pointColor = new Color(0, 230, 0);
        series2.lineColor = new Color(0, 230, 0);

        series2.enabled = rec_enabled;

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
        data2Store = new List<Vector2>();

        cur_pos = graphGO.GetComponent<Transform>().localPosition;

        //series1.enabled = graph_enabled;
        //graphGO.SetActive(graph_enabled);
        //ToggleGraph();

        StartCoroutine(DisableGraph());

        //series1.pointValues
        //graph.xAxis.AxisMaxValue = x;

    }

    public IEnumerator DisableGraph()
    {
        yield return new WaitForSeconds(0.15f);
        //graphGO.SetActive(false);
        ToggleGraph();
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
        graph_enabled = !graphGO.activeSelf;
        series1.enabled = graph_enabled;
        series2.enabled = graph_enabled;
        graphGO.SetActive(graph_enabled);
    }

    public void SetGraph(bool graph_enable)
    {
        series1.enabled = graph_enabled;
        series2.enabled = graph_enabled;
        graphGO.SetActive(graph_enable);
    }

    public void ToggleMode()
    {
        analysis_mode = !analysis_mode;
        Vector2 g_scale = graphGO.GetComponent<RectTransform>().sizeDelta;
        if (analysis_mode)
        {
            graphGO.GetComponent<Transform>().localPosition = alt_pos;
            graphGO.GetComponent<RectTransform>().sizeDelta = new Vector2(g_scale.x * pointBuffer, g_scale.y);
            graph.xAxis.AxisMinValue = graph.xAxis.AxisMinValue * pointBuffer;
            series1.pointValues.SetList(dataStore);
            if (rec_enabled)
                series2.pointValues.SetList(data2Store);
        }
        else
        {
            graphGO.GetComponent<Transform>().localPosition = cur_pos;
            graphGO.GetComponent<RectTransform>().sizeDelta = new Vector2(g_scale.x / pointBuffer, g_scale.y);
            graph.xAxis.AxisMinValue = graph.xAxis.AxisMinValue / pointBuffer;
            series1.pointValues.SetList(data);
            if(rec_enabled)
                series2.pointValues.SetList(data2);
        }
        
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

        for (int i = 0; i < dataStore.Count; i++)
        {
            Vector2 prevValue = dataStore[i];
            dataStore.Insert(i, new Vector2(prevValue.x + timestep, prevValue.y));
            dataStore.RemoveAt(i + 1);

            prevValue = data2Store[i];
            data2Store.Insert(i, new Vector2(prevValue.x + timestep, prevValue.y));
            data2Store.RemoveAt(i + 1);

        }

        data.Insert(0, new Vector2(0f, (float)volt[band_width]));
        dataStore.Insert(0, new Vector2(0f, (float)volt[band_width]));
        if (rec_enabled)
        {
            data2.Insert(0, new Vector2(0f, (float)volt[recordingSite]));
            data2Store.Insert(0, new Vector2(0f, (float)volt[recordingSite]));
        }
        else
        {
            data2.Insert(0, new Vector2(0f, 0f));
            data2Store.Insert(0, new Vector2(0f, 0f));
        }

        if (data.Count >= num_points)
        {
            data.RemoveAt(num_points - 1);
            data2.RemoveAt(num_points - 1);
        }

        if (dataStore.Count >= num_points * pointBuffer)
        {
            dataStore.RemoveAt(num_points*pointBuffer - 1);
            data2Store.RemoveAt(num_points * pointBuffer - 1);
        }

        if (analysis_mode)
        {
            series1.pointValues.SetList(dataStore);
            if (rec_enabled)
                series2.pointValues.SetList(data2Store);
            else
                series2.pointValues.SetList(new List<Vector2>());
        }
        else
        {
            series1.pointValues.SetList(data);
            if (rec_enabled)
                series2.pointValues.SetList(data2);
            else
                series2.pointValues.SetList(new List<Vector2>());
        }

    }

}

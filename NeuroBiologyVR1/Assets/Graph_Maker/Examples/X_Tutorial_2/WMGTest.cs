using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WMGTest : MonoBehaviour
{

    public GameObject emptyGraphPrefab;
    public GameObject CubeObject;
    public WMG_Axis_Graph graph;
    public WMG_Series series1;
    public WMG_Series series2;


    private ChangeColor otherScript;
    //private double [,] newMatTwo;
    private double[] newMatTwo;
    private int x = 20;
    private int t = 20;
    private int recordingSite = 10;

    private float coroutine_time;
    private float fupdate_time = 0.02f;
    private float update_count = 0f;

    public List<Vector2> data, data2;


    void Start()
    {
        GameObject graphGO = GameObject.Instantiate(emptyGraphPrefab);
        graphGO.transform.SetParent(this.transform, false);
        graph = graphGO.GetComponent<WMG_Axis_Graph>();
        series1 = graph.addSeries();
        series2 = graph.addSeries();
        series2.pointColor = new Color(0, 230, 0);
        series2.lineColor = new Color(0, 230, 0);

        graph.xAxis.AxisMinValue = 20;
        graph.xAxis.AxisMaxValue = 0;
        graph.autoUpdateOrigin = false;
        graph.theOrigin = new Vector2(0f, 0f);
        graph.xAxis.AxisTitleString = "Time Passed (μs)";
        graph.autoShrinkAtPercent = 0f;
        graph.autoAnimationsEnabled = false;

        coroutine_time = 0.15f;//otherScript.coroutine_wait;

        otherScript = CubeObject.GetComponent<ChangeColor>();

        data = new List<Vector2>();
        data2 = new List<Vector2>();

        //graph.xAxis.AxisMaxValue = x;

    }


    public void RecieveSliderValue(float valuez)
    {
        recordingSite = (int)valuez;

    }

    // Use this for initialization
    void Update()
    {

        if (update_count >= coroutine_time)
        {
            update_count = update_count % coroutine_time;

            //otherScript = CubeObject.GetComponent<ChangeColor>();

            //VTwo = otherScript.V;
            //
            //Refresh();


            otherScript.UpdateRecorders();
            recordingSite = otherScript.recordingSite;

            //newMatTwo = otherScript.newMat;
            newMatTwo = otherScript.cont_voltage;

            //List<Vector2> data = new List<Vector2>();
            //List<Vector2> data2 = new List<Vector2>();

            for (int i = 0; i < data.Count; i++)
            {

                float temp_x = data[i].x;
                //Debug.Log(temp_x);
                data.Insert(i, new Vector2(temp_x + 1, data[i].y));
                data.RemoveAt(i + 1);
                //data[i].Set(temp_x + 1, data[i].y);
                //data2[i].Set(temp_x + 1, data2[i].y);
                data2.Insert(i, new Vector2(temp_x + 1, data2[i].y));
                data2.RemoveAt(i + 1);
            }

            data.Insert(0, new Vector2(0f, (float)newMatTwo[20]));
            data2.Insert(0, new Vector2(0f, (float)newMatTwo[x + recordingSite]));

            /**/
            if (data.Count >= t)
            {

                data.RemoveAt(t - 1);
                data2.RemoveAt(t - 1);
            }/**/

            series1.pointValues.SetList(data);
            series2.pointValues.SetList(data2);
            /*
            for (int i = 0; i < t - 1; i++)
            {

                //data.Add(new Vector2((float)i, (float)newMatTwo[i, 20]));
                data.Add(new Vector2((float)i, (float)newMatTwo[i, 20]));
            }
            
            series1.pointValues.SetList(data);

            for (int i = 0; i < t - 1; i++)
            {

                data2.Add(new Vector2((float)i, (float)newMatTwo[i, x + recordingSite]));
            }

            series2.pointValues.SetList(data2);
            */

        }
        else
        {
            update_count += fupdate_time;
        }
    }

}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    static Text slotTime;
    static float currentTime = 0.0000f;
    public int Interval;
    // Use this for initialization
    void Awake()
    {
        //BeginTimer
        StartCoroutine(UpdateTime());
    }
    void Update()
    {
        StartCoroutine(UpdateTime());
    }
    private IEnumerator UpdateTime()
    {
        slotTime = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        yield return new WaitForSeconds(Interval);
        currentTime += Time.time;
        slotTime.text = "" + (Time.time-23.0) * .001;
    
        for (int i = 0; i <= slotTime.text.Length; i++)
        {
           
            if (slotTime.text.Substring(i) == "0")
            {
                slotTime.text.Substring(i).Equals("<color=grey>0</color=grey>");
                print(slotTime.text.Substring(i));
            }
            
        }
    }
}


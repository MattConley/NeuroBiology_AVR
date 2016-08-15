using UnityEngine;
using System.Collections;

public class AutomaticLoad : MonoBehaviour {
    static float currentTime = 0.0000f;
    public int level;
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
        currentTime += Time.time;
        
        if(currentTime == 3.0)
        {
            Application.LoadLevel(level);
            yield return null;
        }
    }
}

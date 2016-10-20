using UnityEngine;
using System.Collections;

public class TimerBehavior : MonoBehaviour {

    public ParticleSystem part_pipette;
    private bool isEnabled;


    // Use this for initialization
    void Start()
    {
        isEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if (isEnabled)
        {
            Time.timeScale = 1.0f + Time.timeScale;
            Debug.Log(Time.timeScale);
        }
    }
}

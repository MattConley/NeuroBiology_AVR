using UnityEngine;
using System.Collections;

public class TimeScaler : MonoBehaviour
{

    void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.P))
        {
            Time.timeScale = 1.0f - Time.timeScale;

            Debug.Log(Time.timeScale);
        }
        else if (Input.GetKey(KeyCode.O))
        {
            Time.timeScale = 1.0f + 50;

            Debug.Log(Time.timeScale);
        }
        else
        {
            {
                Time.timeScale = 1.0f;

                Debug.Log(Time.timeScale);
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class GateController : MonoBehaviour
{
    public float rotx;
    public float roty;
    public float rotz;
    public float interval;
    public float delay;
    public int number;
    public float wait;
    public float closeTime;
    public GameObject Player;
    // public GameObject Camera;
    // Use this for initialization
    void Awake()
    {

        StartCoroutine(Animation());
    }

    // Update is called once per frame
    private IEnumerator Animation()
    {
        int count = 0;
        
        yield return new WaitForSeconds(delay);
        while (count < number)
        {
            float rate = 1.0f / closeTime;
            float progress = 0.0f;
            while (progress < 1)
            {
                Player.GetComponent<Rigidbody>().transform.Rotate(rotx, roty, rotz);
                progress += rate;
                yield return null;
            }

            yield return new WaitForSeconds(interval);
            rate = 1.0f / closeTime;
            progress = 0.0f;
            while (progress < 1)
            {
                Player.GetComponent<Rigidbody>().transform.Rotate(-rotx, -roty, -rotz);
                progress += rate;
                yield return null;
            }
            yield return new WaitForSeconds(wait);
        }
    }
}


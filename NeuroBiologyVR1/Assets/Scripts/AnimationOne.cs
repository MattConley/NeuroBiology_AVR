using UnityEngine;
using System.Collections;
using System;

public class AnimationOne : MonoBehaviour
{
    public int velocity;
    public float timer;
    public float rot;
    public GameObject Player;
    public GameObject Camera;
    // Use this for initialization
    void Awake()
    {

        StartCoroutine(Animation());
    }


    // Update is called once per frame
    private IEnumerator Animation()
    {

        float rate = 0.0f;

        while (rate < timer)
        {
            Player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, velocity);

            rate += Time.deltaTime;
            yield return null;
        }
    Camera.GetComponent<Transform>().forward = new Vector3(0, Camera.GetComponent<Transform>().right.y - 90, 0);
    }
}   


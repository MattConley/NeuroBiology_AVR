using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class StimulateCell : MonoBehaviour
{
    public GameObject DustStormSpawn;

    void Start()
    {
        GetComponent<ParticleSystem>();
    }
}
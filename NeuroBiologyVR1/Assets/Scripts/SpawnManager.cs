using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SpawnManager : MonoBehaviour
{
    public int initialVelocity = 10;
    public GameObject IonSpawn;
    public Transform SpawnPosition;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject ionInstance;
            ionInstance = Instantiate(IonSpawn, SpawnPosition.position, SpawnPosition.rotation) as GameObject;
            ionInstance.AddComponent<Rigidbody>();
            ionInstance.GetComponent<Rigidbody>().velocity = new Vector3(0, 10, 0);
         }
    }
}


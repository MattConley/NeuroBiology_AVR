using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SpawnManager : MonoBehaviour
{
    public int initialVelocity = 3;
    public GameObject IonSpawn;
    public Transform SpawnPosition;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject ionInstance;
            ionInstance = Instantiate(IonSpawn, SpawnPosition.position, SpawnPosition.rotation) as GameObject;
            //ionInstance.AddComponent<Rigidbody>();
            ionInstance.GetComponent<Rigidbody>().velocity = new Vector3(0, -10, 0);
            ionInstance.transform.SetParent(GameObject.Find("PositiveIons").transform);
            this.GetComponent<ParticleManager>().addParticle(ionInstance.GetComponent<MovingChargedParticle>());
            //TO KEEP OUTSIDE CHARGE UNAFFECTED BY THE SPAWNING IN OF POSITIVE IONS
            ScoreTracker.UpdateOutsideDistribution(1);
            
        }
    }
}


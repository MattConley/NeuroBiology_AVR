using UnityEngine;
using System.Collections;

public class CableStimBehavior : MonoBehaviour
{
    
    private bool isEnabled;
    public int initialVelocity;
    public GameObject IonSpawn;
    public Transform SpawnPosition;
    public float spawnRate;
    private float nextSpawn = 0.0F;

    // Use this for initialization
    public void Start()
    {
        isEnabled = true;
    }

    public void OnMouseDown()
    {
        if (isEnabled)
        {
            GameObject ionInstance;
            ionInstance = Instantiate(IonSpawn, SpawnPosition.position, SpawnPosition.rotation) as GameObject;
            //ionInstance.AddComponent<Rigidbody>();
            ionInstance.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-10, 10), initialVelocity, Random.Range(-10, 10));
            ionInstance.transform.SetParent(GameObject.Find("PositiveIons").transform);
            this.GetComponent<ParticleManager>().addParticle(ionInstance.GetComponent<MovingChargedParticle>());
            //TO KEEP OUTSIDE CHARGE UNAFFECTED BY THE SPAWNING IN OF POSITIVE IONS
           // ScoreTracker.UpdateOutsideDistribution(1);
        }
    }
}

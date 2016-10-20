using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PatchSManager : MonoBehaviour
{
    public int initialVelocity;
    public GameObject IonSpawn;
    public Transform SpawnPosition;
    public float fireRate;
    private float nextFire = 0.0F;
    void Update()
    {

        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject ionInstance;
            ionInstance = Instantiate(IonSpawn, SpawnPosition.position, SpawnPosition.rotation) as GameObject;
            //ionInstance.AddComponent<Rigidbody>();
            ionInstance.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-10, 10), initialVelocity, Random.Range(-10, 10));
            ionInstance.transform.SetParent(GameObject.Find("PositiveIons").transform);
            this.GetComponent<ParticleManager>().addParticle(ionInstance.GetComponent<MovingChargedParticle>());
            //TO KEEP OUTSIDE CHARGE UNAFFECTED BY THE SPAWNING IN OF POSITIVE IONS
            ScoreTracker.UpdateInsideDistribution(1);
        }

    }

}


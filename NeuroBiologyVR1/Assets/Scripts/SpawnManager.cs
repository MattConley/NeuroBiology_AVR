using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public int initialVelocity;
    public GameObject IonSpawn;
    public Transform SpawnPosition;
    public float spawnRate;
    private float nextSpawn = 0.0F;
    //-------------------------------------------------------
    /*
    public int pooledAmount = 80;
    public List<GameObject> postiveIons;
    public List<PostiveIonTrigger> Lattice;
    

    void Start()
    {
        Lattice = new List<PostiveIonTrigger>(FindObjectsOfType<PostiveIonTrigger>());
        createPositiveIonList();
    }
    public void createPositiveIonList()
    {
        postiveIons = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {

            if (i < 54)
            {
                GameObject existingIons = Lattice[i].GetComponent<GameObject>();
                existingIons.SetActive(true);
                postiveIons.Add(existingIons);
            }
            else if (i >= 54)
            {
                GameObject ionInstance = (GameObject)Instantiate(IonSpawn);
                ionInstance.SetActive(false);
                postiveIons.Add(ionInstance);
            }
        }
    }

    //-------------------------------------------------------
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnRate;
            for (int i=0; i< postiveIons.Count; i++)
            {
                if (!postiveIons[i].activeInHierarchy)
                {
                    postiveIons[i].SetActive(true);
                    postiveIons[i].transform.position = SpawnPosition.position;
                    postiveIons[i].transform.rotation = SpawnPosition.rotation;
                    postiveIons[i].GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-10, 10), initialVelocity, Random.Range(-10, 10));
                    GetComponent<ParticleManager>().addParticle(postiveIons[i].GetComponent<MovingChargedParticle>());
                    postiveIons[i].transform.SetParent(GameObject.Find("PositiveIons").transform);
                    ScoreTracker.UpdateOutsideDistribution(1);
                    break;
                }
            }
        }
    }*/
    void Spawn()
    {

        //if (Input.GetMouseButton(0) && Time.time > nextSpawn)
       // {
       //     nextSpawn = Time.time + spawnRate;
            GameObject ionInstance;
            ionInstance = Instantiate(IonSpawn, SpawnPosition.position, SpawnPosition.rotation) as GameObject;
            //ionInstance.AddComponent<Rigidbody>();
            ionInstance.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-10, 10), initialVelocity, Random.Range(-10,10));
            ionInstance.transform.SetParent(GameObject.Find("PositiveIons").transform);
            this.GetComponent<ParticleManager>().addParticle(ionInstance.GetComponent<MovingChargedParticle>());
            //TO KEEP OUTSIDE CHARGE UNAFFECTED BY THE SPAWNING IN OF POSITIVE IONS
            ScoreTracker.UpdateOutsideDistribution(1);
       // }

    }
    /**/

}


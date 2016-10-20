using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleManager : MonoBehaviour
{
    private float samplingInterval = 0.01f;
    public List<ChargedParticle> chargedParticles;
    public List<MovingChargedParticle> movingChargedParticles;

    // Use this for initialization
    void Start()
    {
        createList();
    }
    public void createList()
    {
        chargedParticles = new List<ChargedParticle>(FindObjectsOfType<ChargedParticle>());
        movingChargedParticles = new List<MovingChargedParticle>(FindObjectsOfType<MovingChargedParticle>());

        foreach (MovingChargedParticle mcp in movingChargedParticles)
        {
            StartCoroutine(Cycle(mcp));
        }
    }
    public void addParticle(MovingChargedParticle mcp)
    {
        chargedParticles.Add(mcp);
        movingChargedParticles.Add(mcp);
        createList();
    }
    public void minusParticle(MovingChargedParticle mcp)
    {
        chargedParticles.Remove(mcp);
        movingChargedParticles.Remove(mcp);
        //Destroy(mcp);
        //createList();
    }
    public IEnumerator Cycle(MovingChargedParticle mcp)
    {
        bool isFirst = true;
        while (mcp != null)
        {
            if (isFirst)
            {
                isFirst = false;
                yield return new WaitForSeconds(Random.Range(0, samplingInterval));
            }
            ApplyMagneticForce(mcp);
            yield return new WaitForSeconds(samplingInterval);
        }
    }

    private void ApplyMagneticForce(MovingChargedParticle mcp)
    {
        Vector3 newForce = Vector3.zero;
        foreach (ChargedParticle cp in chargedParticles)
        {
            if(mcp == cp)
            {
            continue;
            }

            if (mcp == null)
            {
                continue;
            }

            float distance = Vector3.Distance(mcp.transform.position, cp.gameObject.transform.position); // Distance between two charged particles.
            float force = 1000 * mcp.charge * cp.charge / Mathf.Pow(distance, 2); //Relatively good scale number, and F = (q*q)/d^2 (Coulombs Law).

            Vector3 direction = mcp.transform.position - cp.transform.position;
            direction.Normalize(); //Just care for the direction so magnitude does not matter here.

            newForce += force * direction * samplingInterval; //new force vector that results from all the different forces *directions * interval.


            if (float.IsNaN(newForce.x))
            {
                newForce = Vector3.zero;
            }
            mcp.rb.AddForce(newForce);

        }
    }

}

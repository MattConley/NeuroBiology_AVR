using UnityEngine;
using System.Collections;

public class PatchParticleSystem : MonoBehaviour {

	public ParticleSystem ionOut;
    public ParticleSystem ionStop;
    public ParticleSystem ionLeft;

    public void IonOut()
    {
        ParticleSystem pSystem = (ParticleSystem)Instantiate(ionOut, ionOut.transform.position, ionOut.transform.rotation);
        pSystem.Play();
    }
    public void IonStop()
    {
        ParticleSystem pSystem = (ParticleSystem)Instantiate(ionStop, ionStop.transform.position, ionStop.transform.rotation);
        pSystem.Play();
    }
    public void IonLeft()
    {
        ParticleSystem pSystem = (ParticleSystem)Instantiate(ionLeft, ionLeft.transform.position, ionLeft.transform.rotation);
        pSystem.Play();
       
    }
}

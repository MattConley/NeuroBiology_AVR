using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class changePartSysParams : MonoBehaviour {

	// Use this for initialization

	float charge = 2.0f;

	ParticleSystem ps;
	ParticleSystem.Particle[] m_Particles;
	private Color32 color;
	float probStop = 0.5f;
	float delta1 = 0.001f; 
	float delta2 = 0.01f;
	public float speedFactor = 1.0f;

	private float [] channelLocations = new float[4];
	private int noChannels;


	float xvel, yvel, zvel;

	void Awake () {

		ps = gameObject.GetComponent<ParticleSystem>();
		if (m_Particles == null || m_Particles.Length < ps.maxParticles)
			m_Particles = new ParticleSystem.Particle[ps.maxParticles]; 
		var ma = ps.main;
		ma.startLifetime = charge; 

		noChannels = 4;
		channelLocations [0] = 1.0f;   // arbitrary values for testing
		channelLocations [1] = 2.0f;   // distances along the cable
		channelLocations [2] = 3.0f;
		channelLocations [3] = 4.0f;


	}
		
	void Update()
	{


		int numParticlesAlive = ps.GetParticles (m_Particles);

			for (int i = 0; i < numParticlesAlive; i++) {

			for(int j = 0; j < noChannels; j++)
			{
				if(m_Particles [i].position.z > channelLocations [j] && m_Particles [i].position.z < channelLocations [j] + delta2)
				{	
					zvel = m_Particles [i].velocity.z;

					if (Mathf.Abs (zvel) > delta1) {   // Don't want to change direction more than once!
						if (Random.Range (0.0f, 1.0f) < probStop) {
							xvel = Random.Range (-1.0f, 1.0f);
							yvel = Random.Range (-1.0f, 1.0f);
							zvel = 0.0f; 
								
							m_Particles [i].velocity = new Vector3 (xvel, yvel, zvel).normalized * speedFactor;
						}
					}
				}
					
			}



			// Apply the particle changes to the particle system
			ps.SetParticles(m_Particles, numParticlesAlive);
		}

	}
}
	
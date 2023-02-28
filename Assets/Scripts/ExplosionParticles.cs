using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticles : MonoBehaviour
{
	[SerializeField] private ParticleSystem _debrisParticles;
	[SerializeField] private ParticleSystem _fireParticles;
	[SerializeField] private ParticleSystem _smokeParticles;

	void Start()
	{
		_debrisParticles.Play();
		_fireParticles.Play();
		_smokeParticles.Play();
	}
}

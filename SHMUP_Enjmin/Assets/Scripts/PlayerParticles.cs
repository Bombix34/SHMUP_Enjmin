using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour {

	[SerializeField]
	ParticleSystem dashParticles;
	[SerializeField]
	ParticleSystem dashMoveParticles;

	[SerializeField]
	ParticleSystem lineMovement;

	[SerializeField]
	ParticleSystem movementParticle;

	[SerializeField]
	ParticleSystem stopMovementParticle;

	void Start()
	{
		dashParticles.Stop();
		dashMoveParticles.Stop();
	}

	public void ActiveDashParticles(bool val)
	{
		if(val)
		{
			dashParticles.Play();
			dashMoveParticles.Play();
		}
		else
		{
			dashParticles.Stop();
			dashMoveParticles.Stop();
		}
	}


	public void LaunchBulleStop(bool val)
	{
		if(val)
			stopMovementParticle.Play();
		else
			stopMovementParticle.Stop();
	}

	public void LaunchLineParticle(bool val)
	{
		if(val)
			lineMovement.Play();
		else
			lineMovement.Stop();
	}
}

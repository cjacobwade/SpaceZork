using UnityEngine;
using System.Collections;

public class EngineSwitch : MonoBehaviour 
{
	public bool engineOn = false;

	[SerializeField] AudioSource revSource;
	[SerializeField] AudioSource idleSource;

	[SerializeField] ThrottleMove throttleMove;
	[SerializeField] EngineThrottle throttleAudio;

	[SerializeField] float volumeDelta = 0.7f;

	public void TurnOffEngine()
	{
		if(engineOn)
		{
			StopAllCoroutines();
			StartCoroutine(OnMouseDown());
		}
	}

	IEnumerator OnMouseDown()
	{
		if(engineOn)
		{
			throttleAudio.enabled = false;
			engineOn = false;

			while(idleSource.volume > 0f)
			{
				idleSource.volume -= Time.deltaTime * volumeDelta;
				yield return 0;
			}
			idleSource.volume = 0f;
		}
		else
		{
			throttleAudio.enabled = true;
			idleSource.Play();
			revSource.Play();

			engineOn = true;

			while(idleSource.volume < throttleAudio.maxVolume)
			{
				idleSource.volume += Time.deltaTime * volumeDelta;
				yield return 0;
			}
			idleSource.volume = throttleAudio.maxVolume;
		}
	}
}

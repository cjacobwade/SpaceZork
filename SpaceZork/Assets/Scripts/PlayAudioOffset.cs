using UnityEngine;
using System.Collections;

public class PlayAudioOffset : MonoBehaviour 
{
	[SerializeField] float maxWait;

	// Use this for initialization
	void Awake () 
	{
		Invoke("PlaySound", Random.Range(0f, maxWait));
	}

	void PlaySound()
	{
		GetComponent<AudioSource>().Play();
	}
}

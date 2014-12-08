using UnityEngine;
using System.Collections;

public class EngineThrottle : MonoBehaviour 
{
	public ThrottleMove throttle;
	[SerializeField] EngineSwitch engineSwitch;

	AudioSource source;
	public float maxVolume = 0.8f;
	[SerializeField] float minVolume = 0.15f;

	float prevThrottle = 0f;

	void Awake()
	{
		source = GetComponent<AudioSource>();
	}

	void Update()
	{
		if(throttle.value != prevThrottle)
		{
			source.volume = Mathf.Lerp(minVolume, maxVolume, throttle.value);
			prevThrottle = throttle.value;
		}

		if(engineSwitch.engineOn)
		{
			StorySystem.instance.RegisterInput(StorySystem.InputSource.Thruster, throttle.value);
		}
	}
}

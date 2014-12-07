using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour 
{
	Light myLight;
	float initStrength;

	[SerializeField] Vector2 flickerTimeRange = new Vector2(0.1f, 0.3f);
	[SerializeField] float strengthRange = 0.1f;
	float flickerTimer = 0f;

	bool isFlickering = false;

	void Awake()
	{
		myLight = GetComponent<Light>();
		initStrength = myLight.intensity;

		StartCoroutine(Flicker());
	}

	void Update()
	{
		if(!isFlickering)
		{
			StartCoroutine(Flicker());
		}
	}

	IEnumerator Flicker()
	{
		isFlickering = true;

		float flickerTime = Random.Range(flickerTimeRange.x,
		                                 flickerTimeRange.y);

		float intensityTarget = Random.Range(initStrength - strengthRange,
		                                     initStrength + strengthRange);

		float initFlickerStrength = myLight.intensity;

		while(flickerTimer < flickerTime)
		{
			myLight.intensity = Mathf.Lerp(initFlickerStrength, intensityTarget, flickerTimer/flickerTime);
			flickerTimer += Time.deltaTime;

			yield return 0;
		}

		flickerTimer = 0f;

		isFlickering = false;
		yield return 0;
	}
}

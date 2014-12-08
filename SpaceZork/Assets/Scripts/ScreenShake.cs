using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour 
{
	[SerializeField] Vector2 hShakeForce = new Vector2(0.3f, 1.5f);
	[SerializeField] Vector2 vShakeForce = new Vector2(0.3f, 1.5f);

	[SerializeField] float soundTime = 0.3f;
	float shakeTimer = 0f;

	[SerializeField] Vector2 minShakeTimeRange = new Vector2(0.03f, 0.1f);

	int sourceIndex = 0;
	[SerializeField] AudioSource[] sources;
	[SerializeField] AudioClip[] rattleSounds;

	Quaternion initRot;

	void Awake()
	{
		initRot = transform.rotation;
	}

	public IEnumerator Shake(float shakeTime)
	{
		float soundTimer = 0f;

		while(shakeTimer < shakeTime)
		{
			float minShakeTime = Random.Range(minShakeTimeRange.x, minShakeTimeRange.y);
			float minShakeTimer = 0f;

			Quaternion initMinRot = transform.rotation;
			Quaternion targetRot = initRot * Quaternion.Euler(Random.Range(vShakeForce.x, vShakeForce.y) * Mathf.Sign(Random.Range(-1f, 1f)),
			                                                  Random.Range(hShakeForce.x, hShakeForce.y)* Mathf.Sign(Random.Range(-1f, 1f)), 0f);

			targetRot = Quaternion.Lerp (targetRot, initRot, shakeTimer/shakeTime);

			if(soundTimer > soundTime)
			{
				sources[sourceIndex].clip = rattleSounds[Random.Range(0, rattleSounds.Length)];
				sources[sourceIndex].Play();
				sourceIndex++;
				
				if(sourceIndex >= sources.Length)
				{
					sourceIndex = 0;
				}

				soundTimer = 0f;
			}

			while(minShakeTimer < minShakeTime)
			{
				float shakeAmount = Mathf.Lerp(minShakeTimer/minShakeTime, 0f, shakeTimer/shakeTime);
				transform.rotation = Quaternion.Lerp(initMinRot, targetRot, shakeAmount);
				shakeTimer += Time.deltaTime;
				minShakeTimer += Time.deltaTime;
				soundTimer += Time.deltaTime;
				yield return 0;
			}

			soundTimer += Time.deltaTime;
			shakeTimer += Time.deltaTime;
			yield return 0;
		}

		shakeTimer = 0f;
	}
}

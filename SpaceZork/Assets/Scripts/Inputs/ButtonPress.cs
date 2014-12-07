using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ButtonPress : MonoBehaviour 
{
	[SerializeField] float pressOffset = 1f;

	[SerializeField] float unpressTime = 0.4f;
	float unpressTimer = 0f;

	Vector3 initOffset;

	AudioSource source;

	void Awake()
	{
		source = GetComponent<AudioSource>();
		initOffset = transform.localPosition;
	}

	void OnMouseDown()
	{
		StopAllCoroutines();
		transform.localPosition = Vector3.up * -pressOffset + initOffset;
		source.Play();
	}

	IEnumerator OnMouseUp()
	{
		Vector3 initPos = transform.localPosition;
		unpressTimer = 0f;

		while(unpressTimer < unpressTime)
		{
			transform.localPosition = Vector3.Lerp(initPos, initOffset, unpressTimer/unpressTime);
			unpressTimer += Time.deltaTime;
			yield return 0;
		}

		yield return 0;
	}
}

using UnityEngine;
using System.Collections;

public class OpenCase : MonoBehaviour 
{
	Quaternion initRot;
	Vector3 initMousePos;

	[SerializeField] float openOffset = 115f;
	[SerializeField] float openSpeed = 3f;
	[SerializeField] float defaultOpenTime = 0.7f;

	bool open = false;

	AudioSource source;

	void Awake()
	{
		source = GetComponent<AudioSource>();
		initRot = transform.localRotation;
	}

	void OnMouseDown()
	{
		initMousePos = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
	}

	void OnMouseUp()
	{
		Vector3 endMousePos = Camera.main.ScreenPointToRay(Input.mousePosition).direction;

		Debug.Log(Vector3.Dot(endMousePos - initMousePos, Vector3.up));

		float dragDot = Vector3.Dot(endMousePos - initMousePos, Vector3.up);

		if(dragDot > 0.03f && !open)
		{
			StopAllCoroutines();
			StartCoroutine(ToggleOpen(dragDot, initRot * Quaternion.Euler(openOffset, 0f, 0f)));
		}
		else if(dragDot < -0.03f && open)
		{
			StopAllCoroutines();
			StartCoroutine(ToggleOpen(dragDot, initRot));
		}
	}

	IEnumerator ToggleOpen(float speed, Quaternion endRot)
	{
		Quaternion initOpenRot = transform.localRotation;

		float openTime = (defaultOpenTime * openSpeed)/Mathf.Abs(speed);
		float openTimer = 0f;

		while(openTimer < openTime)
		{
			transform.localRotation = Quaternion.Lerp(initOpenRot, endRot, openTimer/openTime);

			openTimer += Time.deltaTime;
			yield return 0;
		}

		if(speed < 0f)
		{
			source.Play();
		}

		open = !open;

		transform.localRotation = endRot;
	}
}

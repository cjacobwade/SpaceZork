using UnityEngine;
using System.Collections;

public class ThrottleMove : MonoBehaviour 
{
	[SerializeField] float maxOffset = 2f;
	[SerializeField] float moveSpeed = 7f;
	[SerializeField] float stoppingSpeed = 2f;
	float dragDot = 0f;

	[SerializeField] float adjustTime = 1.5f;

	bool inControl = true;

	public float value
	{
		get
		{
			// do this offset as an easy fix for 0 not procing input
			return Mathf.InverseLerp(-maxOffset, maxOffset, transform.localPosition.z) + 0.001f;
		}
	}

	void OnMouseDrag()
	{
		Vector3 dirToThrottle = (transform.position - Camera.main.transform.position).normalized;
		Vector3 currentMousePos = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
		dragDot = Vector3.Dot(currentMousePos - dirToThrottle, transform.forward);
	}

	void Update()
	{
		if(inControl)
		{
			if(dragDot > WadeUtils.SMALLNUMBER || dragDot < -WadeUtils.SMALLNUMBER)
			{
				transform.localPosition += transform.forward * dragDot * moveSpeed;
			}

			if(!Input.GetMouseButton(0))
			{
				dragDot = Mathf.Lerp(dragDot, 0f, Time.deltaTime * stoppingSpeed);
			}
			
			Vector3 localPos = Vector3.zero;
			localPos.z = Mathf.Clamp(transform.localPosition.z, -maxOffset, maxOffset);
			
			transform.localPosition = localPos;
		}
	}

	public void MoveToPercentageAlongThrottle(float percent)
	{
		StopAllCoroutines();
		StartCoroutine(MoveToPercentageAlongThrottleRoutine(percent));
	}

	IEnumerator MoveToPercentageAlongThrottleRoutine(float percent)
	{
		inControl = false;
		percent = Mathf.Clamp(percent, 0f, 1f);
		float adjustTimer = 0f;

		Vector3 initPos = transform.localPosition;
		Vector3 targetPos = Vector3.Lerp(Vector3.forward * -maxOffset, 
		                                      Vector3.forward * maxOffset, 
		                                      percent);

		while(adjustTimer < adjustTime)
		{
			transform.localPosition = Vector3.Lerp(initPos, targetPos, adjustTimer/adjustTime);
			adjustTimer += Time.deltaTime;
			yield return 0;
		}

		transform.localPosition = targetPos;
		inControl = true;
	}
}

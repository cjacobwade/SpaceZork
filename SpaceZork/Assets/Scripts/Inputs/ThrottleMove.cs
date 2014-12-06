using UnityEngine;
using System.Collections;

public class ThrottleMove : MonoBehaviour 
{
	[SerializeField] float maxOffset = 2f;
	[SerializeField] float moveSpeed = 7f;

	Vector3 initMousePos;

	void OnMouseDown()
	{
		initMousePos = Input.mousePosition;
	}

	void OnMouseDrag()
	{
		float camOffset = transform.position.z - Camera.main.transform.position.z;
		Vector3 mouseDeltaWorld = Camera.main.ScreenToWorldPoint(initMousePos + Input.mousePosition);
		Vector3 targetPos = transform.TransformDirection(mouseDeltaWorld + Vector3.forward * camOffset);

		Debug.Log(targetPos);

		targetPos.x = 0f;
		targetPos.z = Mathf.Clamp(targetPos.z, -maxOffset, maxOffset);
		targetPos.y = 0f;

		transform.localPosition = targetPos;
		//transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * moveSpeed);
	}
}

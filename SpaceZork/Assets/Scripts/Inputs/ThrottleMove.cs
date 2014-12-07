using UnityEngine;
using System.Collections;

public class ThrottleMove : MonoBehaviour 
{
	[SerializeField] float maxOffset = 2f;
	[SerializeField] float moveSpeed = 7f;
	[SerializeField] float stoppingSpeed = 2f;
	float dragDot = 0f;

	public float value
	{
		get
		{
			return Mathf.InverseLerp(-maxOffset, maxOffset, transform.localPosition.z);
		}
	}

	Vector3 initMousePos;

	void OnMouseDown()
	{
		initMousePos = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
	}

	void OnMouseDrag()
	{
		Vector3 currentMousePos = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
		dragDot = Vector3.Dot(currentMousePos - initMousePos, transform.forward);

		Debug.Log(dragDot);
	}

	void Update()
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

using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour 
{
	[SerializeField] AnimationCurve hoverCurve;
	[SerializeField] float height = 0.02f;
	[SerializeField] float speed = 1f;

	float timer = 0f;
	Vector3 initPos;

	void Awake()
	{
		initPos = transform.position;
	}
	
	void Update () 
	{
		transform.position = initPos + transform.up * hoverCurve.Evaluate(timer) * height;

		timer += Time.deltaTime * speed;
		if(timer > 1f)
		{
			timer = 0f;
		}
	}
}

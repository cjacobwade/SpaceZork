using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour 
{
	[SerializeField] Transform followTarget;
	[SerializeField] float followSpeed = 7f;

	Vector3 offset;

	void Awake()
	{
		offset = followTarget.position - transform.position;
	}

	void Update()
	{
		transform.position = Vector3.Lerp(transform.position, followTarget.position - offset, Time.deltaTime * followSpeed);
	}
}

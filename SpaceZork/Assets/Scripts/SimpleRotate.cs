using UnityEngine;
using System.Collections;

public class SimpleRotate : MonoBehaviour 
{
	[SerializeField] Vector3 rotationSpeed;

	void Update () 
	{
		transform.localRotation *= Quaternion.Euler(rotationSpeed * Time.deltaTime);
	}
}

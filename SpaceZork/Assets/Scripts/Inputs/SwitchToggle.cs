using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SwitchToggle : MonoBehaviour 
{
	[SerializeField] Transform switchObj;
	[SerializeField] AudioSource source;

	public void OnMouseDown()
	{
		switchObj.transform.localRotation *= Quaternion.Euler(0f, 180f, 0f);
		source.Play();
	}
}

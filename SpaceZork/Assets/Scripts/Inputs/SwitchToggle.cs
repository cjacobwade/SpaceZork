using UnityEngine;
using System.Collections;

public class SwitchToggle : MonoBehaviour 
{
	[SerializeField] Transform switchObj;

	void OnMouseUpAsButton()
	{
		switchObj.transform.localRotation *= Quaternion.Euler(0f, 180f, 0f);
	}
}

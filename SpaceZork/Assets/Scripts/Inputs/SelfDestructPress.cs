using UnityEngine;
using System.Collections;

public class SelfDestructPress : MonoBehaviour 
{
	void OnMouseDown()
	{
		StorySystem.instance.RegisterInput(StorySystem.InputSource.SelfDestruct, 10f);
	}
}

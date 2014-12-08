using UnityEngine;
using System.Collections;

public class LaserPress : MonoBehaviour 
{
	[SerializeField] ThrottleMove throttle;

	void OnMouseDown()
	{
		StorySystem.instance.RegisterInput(throttle.value > 0.4f ? StorySystem.InputSource.DeathLaser : StorySystem.InputSource.StunLaser, 
		                                   throttle.value);
	}
}

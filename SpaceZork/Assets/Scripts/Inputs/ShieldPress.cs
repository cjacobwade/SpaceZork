using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShieldPress : MonoBehaviour 
{
	[SerializeField] Slider slider;
	[SerializeField] Vector2 powerIncRange = new Vector2(0.05f, 0.15f);
	float shieldPower = 0f;

	void OnMouseDown()
	{
		StorySystem.InputOptions inputOptions = StorySystem.instance.GetMyInputOptions(StorySystem.InputSource.ChargeShield);

		if(inputOptions.expectedValue > 0f)
		{
			slider.value += Random.Range(powerIncRange.x, powerIncRange.y);
			shieldPower = slider.value;
		}
		else
		{
			shieldPower = 0f;
		}

		if(inputOptions.expectedValue < shieldPower)
		{
			StorySystem.instance.RegisterInput(StorySystem.InputSource.ChargeShield, shieldPower);
		}
	}
}

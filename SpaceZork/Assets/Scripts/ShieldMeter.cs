using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShieldMeter : MonoBehaviour 
{
	Slider slider;
	[SerializeField] Image image;
	Color initColor;

	[SerializeField] float shieldFallSpeed = 1.5f;

	void Awake()
	{
		slider = GetComponent<Slider>();
		initColor = image.color;
	}

	void Update()
	{
		slider.value -= Time.deltaTime * shieldFallSpeed;
		image.color = Color.Lerp(initColor - new Color(0f, 0f, 0f, 1f), initColor, slider.value);
	}
}

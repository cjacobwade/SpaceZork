using UnityEngine;
using System.Collections;

public class LaserStrength : MonoBehaviour 
{
	public ThrottleMove throttle;
	TextMesh textMesh;

	[SerializeField] float killStrength = 0.5f;
	[SerializeField] float rektStrength = 0.95f;
	
	void Awake()
	{
		textMesh = GetComponent<TextMesh>();
	}

	void Update () 
	{
		if(throttle.value > rektStrength)
		{
			textMesh.text = "REK'T";
			textMesh.color = new Color(1f, .75f, .75f, 1f);
		}
		else if(throttle.value > killStrength)
		{
			textMesh.text = "KILL";
			textMesh.color = Color.red;
		}
		else
		{
			textMesh.text = "STUN";
			textMesh.color = Color.cyan;
		}
	}
}

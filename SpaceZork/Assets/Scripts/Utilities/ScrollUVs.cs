using UnityEngine;
using System.Collections;

public class ScrollUVs : MonoBehaviour 
{
	Vector2 currentOffset;
	[SerializeField] Vector2 uvSpeed;
	Renderer myRenderer;

	void Awake () 
	{
		myRenderer = GetComponent<Renderer>();
		currentOffset = myRenderer.material.GetTextureOffset("_MainTex");
	}

	void Update () 
	{
		currentOffset += uvSpeed * Time.deltaTime;
		myRenderer.material.SetTextureOffset("_MainTex", currentOffset);
	}
}

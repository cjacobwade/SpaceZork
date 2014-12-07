﻿using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]

public class ScreenShader : MonoBehaviour
{
	public Shader shader;
	public Shader shader2;
	private Material _material;
	
	protected Material material
	{
		get
		{
			if (_material == null)
			{
				_material = new Material(shader);
				_material.hideFlags = HideFlags.HideAndDontSave;
			}
			return _material;
		}
	}
	
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (shader == null) return;
		Material mat = material;
		Graphics.Blit(source, destination, mat);
	}
	
	void OnDisable()
	{
		if (_material)
		{
			DestroyImmediate(_material);
		}
	}
}
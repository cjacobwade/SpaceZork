using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class StorySystem : SingletonBehaviour<StorySystem> 
{
	public enum InputSource
	{
		StunLaser		= 0,
		DeathLaser		= 1,
		Thruster		= 2,
		ChargeShield	= 3,
		SelfDestruct	= 4
	}

	public enum InputType
	{
		Binary		= 0,
		Range		= 1,
		Activate	= 2
	}

	public struct InputOptions
	{
		public InputOptions(InputSource inInputSource, float inExpectedValue, int inNextChunkID)
		{
			inputSource = inInputSource;
			expectedValue = inExpectedValue;
			nextChunkID = inNextChunkID;
		}
		
		public InputSource inputSource;
		public float expectedValue;
		public int nextChunkID;
	}

	public struct StoryChunk
	{
		string text;
		int chunkID;
		InputOptions[] inputOptions;
	};

	public Dictionary<InputSource, InputType> inputMap = new Dictionary<InputSource, InputType>();
	Text textField;

	string dialogFileName = "Assets/Text/Dialog.txt";
	SimpleJSON.JSONNode json;

	[SerializeField] TextAsset dialogFile;

	string textToRead = "";
	bool lookForInput = false;
	int currentStoryChunk = 0;

	[SerializeField] AudioSource monitorAudio;
	[SerializeField] AudioClip[] typingSounds;

	[SerializeField] EngineSwitch engineSwitch;
	[SerializeField] SwitchToggle switchToggle;
	[SerializeField] ThrottleMove engineThrottle;

	AudioSource source;
	[SerializeField] float dimTime = 3.5f;

	[SerializeField] Transform title;
	[SerializeField] float titleTime = 3.5f;
	[SerializeField] float titleOffset = 30f;

	[SerializeField] ScreenShake screenShake;

	// Tweakables
	[SerializeField] float charTime = 0.01f;
	[SerializeField] int maxLines = 7;

	void Awake()
	{
		inputMap.Add(InputSource.DeathLaser, InputType.Activate);
		inputMap.Add(InputSource.StunLaser, InputType.Activate);
		inputMap.Add(InputSource.Thruster, InputType.Range);
		inputMap.Add(InputSource.ChargeShield, InputType.Activate);
		inputMap.Add(InputSource.SelfDestruct, InputType.Activate);

		textField = GetComponent<Text>();
		textField.text = "";

		source = GetComponent<AudioSource>();

		ParseDialog();

		StartCoroutine(Read());
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	void ParseDialog()
	{
		string dialogText = dialogFile.text;
		//string dialogText = File.ReadAllText(dialogFileName);
		json = SimpleJSON.JSON.Parse(dialogText);

		textToRead = json["storyChunks"][currentStoryChunk]["text"];
	}
	
	IEnumerator Read()
	{
		yield return new WaitForSeconds(3.5f);

		Vector3 initPos = title.position;
		float titleTimer = 0f;

		while(titleTimer < titleTime)
		{
			title.position = Vector3.Lerp(initPos, initPos + Vector3.up * titleOffset, titleTimer/titleTime);

			titleTimer += Time.deltaTime;
			yield return 0;
		}

		while(true)
		{
			int i = 0;
			while(i < textToRead.Length)
			{
				yield return new WaitForSeconds(charTime);
				
				TextGenerator texGen = textField.cachedTextGenerator;
				if(texGen.lineCount > maxLines)
				{
					yield return new WaitForSeconds(charTime);
					textField.text = textField.text.Remove(0, texGen.lines[1].startCharIdx);
				}

				if(textToRead[i] == '[')
				{
					int commandLength = textToRead.IndexOf("]", i) - i - 1; // don't want brackets

					char[] commandArr = new char[commandLength];
					textToRead.CopyTo(i + 1, commandArr, 0, commandLength);
					string command = new string(commandArr);

					textToRead = textToRead.Remove(i, commandLength + 2); // need to remove end bracket

					if(command.Contains("ReadAtSpeed"))
					{
						command = command.Remove(0, 11);

						float tmpCharTime;
						if(float.TryParse(command, out tmpCharTime))
						{
							charTime = tmpCharTime;
						}
						else
						{
							Debug.LogError("Trying to parse [ReadAtSpeed] speed, but was passed invalid value");
						}
					}
					else if(command.Contains("CallMethod"))
					{
						command = command.Remove(0, 10);
						Invoke(command, 0f);
					}
					else if(command.Contains("Wait"))
					{
						command = command.Remove(0, 4);

						float waitTime;
						if(float.TryParse(command, out waitTime))
						{
							yield return new WaitForSeconds(float.Parse(command));
						}
						else
						{
							Debug.LogError("Trying to parse [Wait] time, but was passed invalid value");
						}
					}
					else if(command.Contains("GoToStoryChunk"))
					{
						command = command.Remove(0, 14);
						
						int storyChunk = 0;
						if(int.TryParse(command, out storyChunk))
						{
							currentStoryChunk = storyChunk;
							textToRead = "\n" + json["storyChunks"][currentStoryChunk]["text"];
							i = 0;
							lookForInput = false;
						}
						else
						{
							Debug.LogError("Trying to parse [GoToStoryChunk] time, but was passed invalid value");
						}
					}
					else if(command.Contains("ScreenShake"))
					{
						command = command.Remove(0, 11);

						float shakeForce;
						if(float.TryParse(command, out shakeForce))
						{
							yield return StartCoroutine(screenShake.Shake(shakeForce));
						}
						else
						{
							Debug.LogError("Trying to parse [Wait] time, but was passed invalid value");
						}
					}
				}

				if(i < textToRead.Length)
				{
					textField.text += textToRead[i];
					monitorAudio.clip = typingSounds[Random.Range(0, typingSounds.Length)];
					monitorAudio.Play();
					i++;
				}
			}

			lookForInput = true;
			
			while(lookForInput)
			{
				// window open for input
				yield return 0;
			}

			i = 0; // got input so go back to reading text
			yield return 0;
		}
	}

	public InputOptions GetMyInputOptions(InputSource source)
	{
		SimpleJSON.JSONNode node = json["storyChunks"][currentStoryChunk]["inputOptions"][(int)source];
		return new InputOptions((InputSource)node["inputSource"].AsInt,
		                        node["expectedValue"].AsFloat,
		                        node["nextChunkID"].AsInt);
	}

	void Quit()
	{
		Debug.Log("Quit");
		Application.Quit();
	}

	void ResetEngine()
	{
		if(engineSwitch.engineOn)
		{
			engineSwitch.TurnOffEngine();
			switchToggle.OnMouseDown();
			engineThrottle.MoveToPercentageAlongThrottle(0f);
		}
	}

	void Dim()
	{
		source.Play();

		foreach(LaserStrength ls in GameObject.FindObjectsOfType<LaserStrength>())
		{
			ls.enabled = false;
		}

		foreach(TextMesh mesh in GameObject.FindObjectsOfType<TextMesh>())
		{
			StartCoroutine(DimText(mesh));
		}

		foreach(Light light in GameObject.FindObjectsOfType<Light>())
		{
			LightFlicker lf = light.gameObject.GetComponent<LightFlicker>();
			if(lf)
			{
				lf.enabled = false;
			}

			StartCoroutine(DimLights(light));
		}
	}

	IEnumerator DimText(TextMesh textMesh)
	{
		float dimTimer = 0f;
		Color initColor = textMesh.color;
		
		while(dimTimer < dimTime)
		{
			textMesh.color = Color.Lerp(initColor, Color.black, dimTimer);
			dimTimer += Time.deltaTime;
			yield return 0;
		}
	}

	IEnumerator DimLights(Light light)
	{
		float dimTimer = 0f;
		float initStrength = light.intensity;

		while(dimTimer < dimTime)
		{
			light.intensity = Mathf.Lerp(initStrength, 0f, dimTimer);
			dimTimer += Time.deltaTime;
			yield return 0;
		}
	}

	public void RegisterInput(InputSource source, float value)
	{
		if(lookForInput)
		{
			SimpleJSON.JSONNode inputOption = json["storyChunks"][currentStoryChunk]["inputOptions"][(int)source];
			if(value >= inputOption["expectedValue"].AsFloat && inputOption["expectedValue"].AsFloat > 0f)
			{
				currentStoryChunk = inputOption["nextChunkID"].AsInt;
				textToRead = "\n" + json["storyChunks"][currentStoryChunk]["text"];
				lookForInput = false;
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class StorySystem : SingletonBehaviour<StorySystem> 
{
	public enum InputSource
	{
		Laser			= 0,
		Thruster		= 1,
		EngineToggle	= 2,
		LaserDamage		= 3,
		ChargeShield	= 4,
		SelfDestruct	= 5
	}

	public enum InputType
	{
		Binary		= 0,
		Range		= 1,
		Activate	= 2
	}

	public struct StoryChunk
	{
		public struct InputOptions
		{
			InputOptions(InputSource inInputSource, float inExpectedValue, int inNextChunkID)
			{
				inputSource = inInputSource;
				expectedValue = inExpectedValue;
				nextChunkID = inNextChunkID;
			}

			InputSource inputSource;
			float expectedValue;
			int nextChunkID;
		}

		string text;
		int chunkID;
		InputOptions[] inputOptions;
	};

	public Dictionary<InputSource, InputType> inputMap = new Dictionary<InputSource, InputType>();
	Text textField;
	int numStoryChunks = 0;

	string dialogFileName = "Assets/Text/Dialog.txt";
	SimpleJSON.JSONNode json;
	StoryChunk[] storyChunks;

	string textToRead = "";

	// Tweakables
	[SerializeField] float charTime = 0.01f;
	[SerializeField] int maxLines = 7;
	
	void Awake()
	{
		inputMap.Add(InputSource.Laser, InputType.Activate);
		inputMap.Add(InputSource.Thruster, InputType.Range);
		inputMap.Add(InputSource.EngineToggle, InputType.Binary);
		inputMap.Add(InputSource.LaserDamage, InputType.Range);
		inputMap.Add(InputSource.ChargeShield, InputType.Activate);
		inputMap.Add(InputSource.SelfDestruct, InputType.Activate);

		textField = GetComponent<Text>();
		textField.text = "";

		ParseDialog();

		StartCoroutine(Read());
	}

	void ParseDialog()
	{
		string dialogText = File.ReadAllText(dialogFileName);
		json = SimpleJSON.JSON.Parse(dialogText);

		textToRead = json["storyChunks"][0]["text"];
	}
	
	IEnumerator Read()
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
					command = command.Remove(0, 4);
					
					int waitTime;
					if(int.TryParse(command, out waitTime))
					{
						//textToRead = GetFromJSON
					}
					else
					{
						Debug.LogError("Trying to parse [Wait] time, but was passed invalid value");
					}
				}
			}

			textField.text += textToRead[i];
			i++;
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StorySystem : SingletonBehaviour<StorySystem> 
{
	//		Inputs:
	//			Laser
	//			Thrust
	//			Toggle engine
	//			Laser damage
	//			Charge shield
	//			Self-destruct

	public enum InputType
	{
		Binary		= 1,
		Range		= 2,
		Activate	= 3
	}

	public struct StoryChunk
	{
		public struct InputOptions
		{
			InputOptions(InputType inInputType, float inExpectedValue, int inNextChunkID)
			{
				inputType = inInputType;
				expectedValue = inExpectedValue;
				nextChunkID = inNextChunkID;
			}

			InputType inputType;
			float expectedValue;
			int nextChunkID;
		}

		string text;
		string functionCall;

		int chunkID;
	};
	
	Text textField;

	string textToRead = "there's a bluebird in my heart that wants to [ReadAtSpeed0.2]get out but I'm too [CallMethodPrintThing]tough for him," +
		"I say, stay in there, I'm not going to [ReadAtSpeed0.05]let anybody see you. there's a bluebird in " +
			"my heart that wants to get out [ReadAtSpeed0.001]but I pour whiskey on him and inhale cigarette smoke " +
			"and the whores and the bartenders and the grocery clerks never know thathe's in there." +
			"there's a bluebird in my heart that wants to get out but I'm too tough for him, I say," +
			"stay down, do you want to mess me up? you want to screw up the works? you want to blow " +
			"my book sales in Europe? there's a bluebird in my heart that wants to get out but I'm too " +
			"clever, I only let him out at night sometimes when everybody's asleep. I say, I know that " +
			"you're there, so don't be sad. then I put him back, but he's singing a little in there, I " +
			"haven't quite let him die and we sleep together like that with our secret pact and it's " +
			"nice enough to make a man weep, but I don't weep, do you?";

	// Tweakables

	[SerializeField] float charTime = 0.01f;
	[SerializeField] int maxLines = 7;
	
	void Awake()
	{
		textField = GetComponent<Text>();
		textField.text = "";
		
		StartCoroutine(Read());
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
					charTime = float.Parse(command);
				}
				else if(command.Contains("CallMethod"))
				{
					command = command.Remove(0, 10);
					Invoke(command, 0f);
				}
			}

			textField.text += textToRead[i];
			i++;
		}
	}

	void PrintThing()
	{
		Debug.Log("This is a thing!");
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReadText : MonoBehaviour 
{
	[SerializeField] float charTime = 0.01f;
	Text textField;

	[SerializeField] int maxLines = 7;

	string textToRead = "there's a bluebird in my heart that wants to get out but I'm too tough for him," +
						"I say, stay in there, I'm not going to let anybody see you. there's a bluebird in " +
						"my heart that wants to get out but I pour whiskey on him and inhale cigarette smoke " +
						"and the whores and the bartenders and the grocery clerks never know thathe's in there." +
						"there's a bluebird in my heart that wants to get out but I'm too tough for him, I say," +
						"stay down, do you want to mess me up? you want to screw up the works? you want to blow " +
						"my book sales in Europe? there's a bluebird in my heart that wants to get out but I'm too " +
						"clever, I only let him out at night sometimes when everybody's asleep. I say, I know that " +
						"you're there, so don't be sad. then I put him back, but he's singing a little in there, I " +
						"haven't quite let him die and we sleep together like that with our secret pact and it's " +
						"nice enough to make a man weep, but I don't weep, do you?";

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
			if(texGen.lineCount > 7)
			{
				yield return new WaitForSeconds(charTime);
				textField.text = textField.text.Remove(0, texGen.lines[1].startCharIdx);
			}

			textField.text += textToRead[i];
			i++;
		}
	}
}

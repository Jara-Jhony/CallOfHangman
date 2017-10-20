using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputManger : MonoBehaviour {

	[SerializeField]
	private InputField myInputField;

	[SerializeField]
	private string currentWord;

	[SerializeField]
	private Text textWordToShow;

	private string wordToShow;

	// Use this for initialization
	void Start () {
		SetSpaceNewWord(currentWord);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown("ENTER"))
		{
			CheckCharInWord(myInputField.textComponent.text, currentWord);

		}
	}

	public void SetSpaceNewWord(string word)
	{
		for(int i = 0; i<word.Length; i++)
		{
			wordToShow += "_";
			
		}
		textWordToShow.text = wordToShow;
	}

	public void CheckCharInWord(string simpleChar, string wordToCheck)
	{
		for(int i = 0; i < wordToCheck.Length; i++)
		{
			if(wordToCheck.Substring(i,1)==simpleChar)
			{
				ChangeChar(i, simpleChar);
				Debug.Log("hapend");
			}
			
		}
			
	}

	public void ChangeChar(int targetIndex, string charToChange)
	{
		string tempString = "";
		for(int i = 0; i < currentWord.Length; i++)
		{	
			if(i != targetIndex)
			{
				tempString += wordToShow.Substring(i,1);
			}else
			{
				tempString += charToChange;
			}
		}
		
		wordToShow = tempString;
		textWordToShow.text = tempString;

	}


}

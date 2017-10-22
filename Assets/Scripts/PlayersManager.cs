using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersManager : MonoBehaviour {


	[SerializeField]
	private string[] wordsForSinglePlayer;

	[SerializeField]
	private GameObject wordSelectionMenu;

	[SerializeField]
	private GameObject playerModeMenu;

	[SerializeField]
	private InputField myInputField;

	[SerializeField]
	private Text textOfWordSelection;

	private bool isFirstPlayersSelected;

	[SerializeField]
	private GameObject[] playersMatchs = new GameObject[2];

	private int numOfTurns;

	private int currentTurn;

	// Use this for initialization
	void Start () {
		
		playersMatchs[0].GetComponent<InputManger>().AdActionPresButton(OnEnterChar);
		playersMatchs[1].GetComponent<InputManger>().AdActionPresButton(OnEnterChar);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetUpSinglePlayer()
	{
		playerModeMenu.SetActive(false);
		numOfTurns = 1;
		playersMatchs[0].GetComponent<InputManger>().SetWordOfGame(wordsForSinglePlayer[Random.Range(0,wordsForSinglePlayer.Length)]);
		ActivateMatches();

	}

	public void SetUpMultiplayer()
	{
		isFirstPlayersSelected = false;
		playerModeMenu.SetActive(false);
		numOfTurns = 2;
		wordSelectionMenu.SetActive(true);
	}

	private void ActivateMatches()
	{
		for(int i = 0; i<numOfTurns; i++)
		{
			playersMatchs[i].SetActive(true);
		}
	}

	public void SelectWord()
	{	
		if(!isFirstPlayersSelected)
		{	
			isFirstPlayersSelected = true;
			playersMatchs[0].GetComponent<InputManger>().SetWordOfGame(myInputField.textComponent.text);
			myInputField.textComponent.text = "";
			textOfWordSelection.text = "Player 1 close your eyes, Player 2 select a word.";
		}else
		{
			playersMatchs[1].GetComponent<InputManger>().SetWordOfGame(myInputField.textComponent.text);
			myInputField.textComponent.text = "";
			textOfWordSelection.text = "Player 2 close your eyes, Player 1 select a word.";
			wordSelectionMenu.SetActive(false);
			playersMatchs[0].GetComponent<InputManger>().SetLockState(true);
			playersMatchs[1].GetComponent<InputManger>().SetLockState(false);
			ActivateMatches();
		}
	}

	public void OnEnterChar()
	{
		switch(numOfTurns)
		{
			case 1:
			break;

			case 2:

			currentTurn++;

			if(currentTurn>=numOfTurns)
				currentTurn = 0;

			SetUpTurn();

			break;

			default:
			break;

		}
	}

	private void SetUpTurn()
	{
		switch(currentTurn)
		{
			case 0:
				playersMatchs[0].GetComponent<InputManger>().SetLockState(true);
				playersMatchs[1].GetComponent<InputManger>().SetLockState(false);
			break;

			case 1:
				playersMatchs[1].GetComponent<InputManger>().SetLockState(true);
				playersMatchs[0].GetComponent<InputManger>().SetLockState(false);
			break;

			default:
			break;
		}
	}
}

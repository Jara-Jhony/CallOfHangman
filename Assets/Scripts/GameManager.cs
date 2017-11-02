using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private Player playerPrefab;
    [SerializeField]
    private PlayerAI playerAIPrefab;

    public int gameMode // 0. No game selected, 1. Singleplayer, 2. Local Multiplayer, 3. Online Multiplayer
    {
        get; private set;
    }

    public int turn
    {
        get; private set;
    }

    public int playerInTurn // 0. Player 1, 1. Player 2
    {
        get; private set;
    }

    public Player[] players
    {
        get; private set;
    }
    //Players indexer
    public Player this[int i]
    {
        get
        {
            return players[i];
        }
        private set
        {
            players[i] = value;
        }
    }

    //Singleton!
    public static GameManager Singleton
    {
        get; private set;
    }

    private void Awake()
    {
        if (Singleton != null)
            DestroyImmediate(gameObject);
        else
            Singleton = this;
    }

    private void Start()
    {
        SuscribeToGameModes();
    }

    public void SetupSingleplayer()
    {
        gameMode = 1;

        SuscribeToSingleplayerEvents();

        UIFacade.Singleton.SetActiveMainMenu(false);
        UIFacade.Singleton.singleplayer.SetActiveSingleplayer(true);

        CreatePlayerWithAI();
    }

    public void SetupLocalMultiplayer()
    {
        gameMode = 2;

        SuscribeToLocalMultiplayerEvents();

        UIFacade.Singleton.SetActiveMainMenu(false);
        UIFacade.Singleton.localMultiplayer.SetActiveLocalMultiplayer(true);

        CreatePlayers();
    }

    public void SetupOnlineMultiplayer()
    {
        gameMode = 3;

        SuscribeToOnlineMultiplayerEvents();

        UIFacade.Singleton.SetActiveMainMenu(false);
        UIFacade.Singleton.onlineMultiplayer.SetActiveOnlineMultiplayer(true);

        CreatePlayers();
    }

    private void SetPlayerWord()
    {
        switch (gameMode)
        {
            case 1:
                players[playerInTurn].SetWord(UIFacade.Singleton.singleplayer.currentInputFieldText);

                if (turn == 0)
                    UIFacade.Singleton.singleplayer.playerTurnInfo.text =
                        string.Format("Player 1 close your eyes, Player 2 select a word.");
                if (turn == 1)
                {
                    UIFacade.Singleton.singleplayer.SetActiveSingleplayerScreen(0, false);
                    UIFacade.Singleton.singleplayer.SetActiveSingleplayerScreen(1, true);
                }
                break;

            case 2:
                players[playerInTurn].SetWord(UIFacade.Singleton.localMultiplayer.currentInputFieldText);

                if (turn == 0)
                    UIFacade.Singleton.localMultiplayer.playerTurnInfo.text =
                        string.Format("Player 1 close your eyes, Player 2 select a word.");
                if (turn == 1)
                {
                    UIFacade.Singleton.localMultiplayer.SetActiveLocalMultiplayerScreen(0, false);
                    UIFacade.Singleton.localMultiplayer.SetActiveLocalMultiplayerScreen(1, true);
                }
                break;

            case 3:
                players[playerInTurn].SetWord(UIFacade.Singleton.onlineMultiplayer.currentInputFieldText);

                if (turn == 0)
                    UIFacade.Singleton.onlineMultiplayer.playerTurnInfo.text =
                        string.Format("Player 1 close your eyes, Player 2 select a word.");
                if (turn == 1)
                {
                    UIFacade.Singleton.onlineMultiplayer.SetActiveOnlineMultiplayerScreen(0, false);
                    UIFacade.Singleton.onlineMultiplayer.SetActiveOnlineMultiplayerScreen(1, true);
                }
                break;
        }
    }

    private void CreatePlayers()
    {
        if (players == null)
            players = new Player[2];

        players[0] = Instantiate(playerPrefab) as Player;
        players[0].SetIndex(0);
        players[1] = Instantiate(playerPrefab) as Player;
        players[1].SetIndex(1);

        players[1].gameObject.SetActive(false);
    }

    private void CreatePlayerWithAI()
    {
        if (players == null)
            players = new Player[2];

        players[0] = Instantiate(playerPrefab) as Player;
        players[0].SetIndex(0);

        PlayerAI playerAI = Instantiate(playerAIPrefab) as PlayerAI;

        players[1] = playerAI;
        players[1].SetIndex(1);

        players[1].gameObject.SetActive(false);
    }

    private void CheckForCharOnRivalPlayerWord()
    {
        switch (gameMode)
        {
            case 1:
                CheckForCharOnRivalPlayerWordCaseSingleplayer();
                break;

            case 2:
                CheckForCharOnRivalPlayerWordCaseLocalMultiplayer();
                break;

            case 3:
                CheckForCharOnRivalPlayerWordCaseOnlineMultiplayer();
                break;

            default:
                return;
        }
    }

    private void CheckForCharOnRivalPlayerWordCaseSingleplayer()
    {
        int otherPlayer = (playerInTurn + 1) % 2;

        //Played char taken from the input field
        char playedChar = UIFacade.Singleton.singleplayer.currentInputFieldText[0];

        //Data of correct chars
        Dictionary<int, char> correctChars =
            players[otherPlayer].CheckForCharsInWord(playedChar);

        //Is the char alredy played ???
        if (players[playerInTurn].playedChars.Contains(playedChar))
            return;
        else
            players[playerInTurn].playedChars.Add(playedChar);

        if (correctChars.Count == 0)
        {
            players[playerInTurn].IncreaseErrorsCount();

            UIFacade.Singleton.singleplayer.UpdateErrors(playerInTurn, players[playerInTurn].errorsCount);

            return;
        }

        if (players[playerInTurn].errorsCount >= 10)
            GameOver(otherPlayer);

        //Updating on the UI
        for (int i = 0; i < players[otherPlayer].wordCharsArray.Length; i++)
        {
            if (correctChars.ContainsKey(i))
            {
                players[playerInTurn].IncreaseSuccessCount();

                switch (otherPlayer)
                {
                    case 0:
                        UIFacade.Singleton.singleplayer.playerOneEmptyTexts[i].text = players[0].wordCharsArray[i].ToString();
                        break;

                    case 1:
                        UIFacade.Singleton.singleplayer.playerTwoEmptyTexts[i].text = players[1].wordCharsArray[i].ToString();
                        break;

                    default:
                        break;
                }
            }
        }

        if (players[playerInTurn].sucessCount ==
            players[otherPlayer].wordCharsArray.Length)
        {
            GameOver(playerInTurn);
        }
    }

    private void CheckForCharOnRivalPlayerWordCaseLocalMultiplayer()
    {
        int otherPlayer = (playerInTurn + 1) % 2;

        //Played char taken from the input field
        char playedChar = UIFacade.Singleton.localMultiplayer.currentInputFieldText[0];

        //Data of correct chars
        Dictionary<int, char> correctChars =
            players[otherPlayer].CheckForCharsInWord(playedChar);

        //Is the char alredy played ???
        if (players[playerInTurn].playedChars.Contains(playedChar))
            return;
        else
            players[playerInTurn].playedChars.Add(playedChar);

        if (correctChars.Count == 0)
        {
            players[playerInTurn].IncreaseErrorsCount();

            UIFacade.Singleton.localMultiplayer.UpdateErrors(playerInTurn, players[playerInTurn].errorsCount);

            return;
        }

        if (players[playerInTurn].errorsCount >= 10)
            GameOver(otherPlayer);

        //Updating on the UI
        for (int i = 0; i < players[otherPlayer].wordCharsArray.Length; i++)
        {
            if (correctChars.ContainsKey(i))
            {
                players[playerInTurn].IncreaseSuccessCount();

                switch (otherPlayer)
                {
                    case 0:
                        UIFacade.Singleton.localMultiplayer.playerOneEmptyTexts[i].text = players[0].wordCharsArray[i].ToString();
                        break;

                    case 1:
                        UIFacade.Singleton.localMultiplayer.playerTwoEmptyTexts[i].text = players[1].wordCharsArray[i].ToString();
                        break;

                    default:
                        break;
                }
            }
        }

        if (players[playerInTurn].sucessCount ==
            players[otherPlayer].wordCharsArray.Length)
        {
            GameOver(playerInTurn);
        }
    }

    private void CheckForCharOnRivalPlayerWordCaseOnlineMultiplayer()
    {
        int otherPlayer = (playerInTurn + 1) % 2;

        //Played char taken from the input field
        char playedChar = UIFacade.Singleton.onlineMultiplayer.currentInputFieldText[0];

        //Data of correct chars
        Dictionary<int, char> correctChars =
            players[otherPlayer].CheckForCharsInWord(playedChar);

        //Is the char alredy played ???
        if (players[playerInTurn].playedChars.Contains(playedChar))
            return;
        else
            players[playerInTurn].playedChars.Add(playedChar);

        if (correctChars.Count == 0)
        {
            players[playerInTurn].IncreaseErrorsCount();

            UIFacade.Singleton.onlineMultiplayer.UpdateErrors(playerInTurn, players[playerInTurn].errorsCount);

            return;
        }

        if (players[playerInTurn].errorsCount >= 10)
            GameOver(otherPlayer);

        //Updating on the UI
        for (int i = 0; i < players[otherPlayer].wordCharsArray.Length; i++)
        {
            if (correctChars.ContainsKey(i))
            {
                players[playerInTurn].IncreaseSuccessCount();

                switch (otherPlayer)
                {
                    case 0:
                        UIFacade.Singleton.onlineMultiplayer.playerOneEmptyTexts[i].text = players[0].wordCharsArray[i].ToString();
                        break;

                    case 1:
                        UIFacade.Singleton.onlineMultiplayer.playerTwoEmptyTexts[i].text = players[1].wordCharsArray[i].ToString();
                        break;

                    default:
                        break;
                }
            }
        }

        if (players[playerInTurn].sucessCount ==
            players[otherPlayer].wordCharsArray.Length)
        {
            GameOver(playerInTurn);
        }
    }

    private void NextTurn()
    {
        turn++;

        if (playerInTurn == 0)
            Observer.Singleton.PlayerOneEndsTurn();
        else
            Observer.Singleton.PlayerTwoEndsTurn();

        playerInTurn = turn % 2;
    }

    private void SuscribeToGameModes()
    {
        //Subscribing to the game modes
        Observer.Singleton.onSingleplayer += SetupSingleplayer;
        Observer.Singleton.onLocalMultiplayer += SetupLocalMultiplayer;
        Observer.Singleton.onOnlineMultiplayer += SetupOnlineMultiplayer;
    }

    private void SuscribeToSingleplayerEvents()
    {
        //Subscribing to the word input field events
        Observer.Singleton.onWordInputFieldEnterSingleplayer += SetPlayerWord;
        Observer.Singleton.onWordInputFieldEnterSingleplayer += NextTurn;
        Observer.Singleton.onWordInputFieldEnterSingleplayer += UIFacade.Singleton.singleplayer.ClearInputFields;
        //Subscribing to the letter input field events
        Observer.Singleton.onLetterInputFieldEnterSingleplayer += CheckForCharOnRivalPlayerWord;
        Observer.Singleton.onLetterInputFieldEnterSingleplayer += NextTurn;
        Observer.Singleton.onLetterInputFieldEnterSingleplayer += UIFacade.Singleton.singleplayer.ClearInputFields;
    }

    private void SuscribeToLocalMultiplayerEvents()
    {
        //Subscribing to the word input field events
        Observer.Singleton.onWordInputFieldEnterLocalMultiplayer += SetPlayerWord;
        Observer.Singleton.onWordInputFieldEnterLocalMultiplayer += NextTurn;
        Observer.Singleton.onWordInputFieldEnterLocalMultiplayer += UIFacade.Singleton.localMultiplayer.ClearInputFields;
        //Subscribing to the letter input field events
        Observer.Singleton.onLetterInputFieldEnterLocalMultiplayer += CheckForCharOnRivalPlayerWord;
        Observer.Singleton.onLetterInputFieldEnterLocalMultiplayer += NextTurn;
        Observer.Singleton.onLetterInputFieldEnterLocalMultiplayer += UIFacade.Singleton.localMultiplayer.ClearInputFields;
    }

    private void SuscribeToOnlineMultiplayerEvents()
    {
        //Subscribing to the word input field events
        Observer.Singleton.onWordInputFieldEnterOnlineMultiplayer += SetPlayerWord;
        Observer.Singleton.onWordInputFieldEnterOnlineMultiplayer += NextTurn;
        Observer.Singleton.onWordInputFieldEnterOnlineMultiplayer += UIFacade.Singleton.onlineMultiplayer.ClearInputFields;
        //Subscribing to the letter input field events
        Observer.Singleton.onLetterInputFieldEnterOnlineMultiplayer += CheckForCharOnRivalPlayerWord;
        Observer.Singleton.onLetterInputFieldEnterOnlineMultiplayer += NextTurn;
        Observer.Singleton.onLetterInputFieldEnterOnlineMultiplayer += UIFacade.Singleton.onlineMultiplayer.ClearInputFields;
    }

    private void GameOver(int winner)
    {
        switch (gameMode)
        {
            case 1:
                UIFacade.Singleton.singleplayer.SetWinnerScreen(winner);
                UIFacade.Singleton.singleplayer.SetActiveSingleplayerScreen(1, false);
                UIFacade.Singleton.singleplayer.SetActiveSingleplayerScreen(2, true);
                break;

            case 2:
                UIFacade.Singleton.localMultiplayer.SetWinnerScreen(winner);
                UIFacade.Singleton.localMultiplayer.SetActiveLocalMultiplayerScreen(1, false);
                UIFacade.Singleton.localMultiplayer.SetActiveLocalMultiplayerScreen(2, true);
                break;

            case 3:
                UIFacade.Singleton.onlineMultiplayer.SetWinnerScreen(winner);
                UIFacade.Singleton.onlineMultiplayer.SetActiveOnlineMultiplayerScreen(1, false);
                UIFacade.Singleton.onlineMultiplayer.SetActiveOnlineMultiplayerScreen(2, true);
                break;

            default:
                break;
        }
    }
}

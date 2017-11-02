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
        //Subscribing to the game modes
        Observer.Singleton.onSingleplayer += SetupSingleplayer;
        Observer.Singleton.onLocalMultiplayer += SetupLocalMultiplayer;
        Observer.Singleton.onOnlineMultiplayer += SetupOnlineMultiplayer;
        //Subscribing to the word input field events
        Observer.Singleton.onWordInputFieldEnter += SetPlayerWord;
        Observer.Singleton.onWordInputFieldEnter += NextTurn;
        Observer.Singleton.onWordInputFieldEnter += UIFacade.Singleton.ClearInputFields;
        //Subscribing to the letter input field events
        Observer.Singleton.onLetterInputFieldEnter += CheckForCharOnRivalPlayerWord;
        Observer.Singleton.onLetterInputFieldEnter += NextTurn;
        Observer.Singleton.onLetterInputFieldEnter += UIFacade.Singleton.ClearInputFields;
    }

    public void SetupSingleplayer()
    {
        gameMode = 1;

        UIFacade.Singleton.SetActiveMainMenu(false);
        UIFacade.Singleton.SetActiveSingleplayer(true);

        CreatePlayerWithAI();
    }

    public void SetupLocalMultiplayer()
    {
        gameMode = 2;

        UIFacade.Singleton.SetActiveMainMenu(false);
        UIFacade.Singleton.SetActiveLocalMultiplayer(true);

        CreatePlayers();
    }

    public void SetupOnlineMultiplayer()
    {
        gameMode = 3;

        UIFacade.Singleton.SetActiveMainMenu(false);
        UIFacade.Singleton.SetActiveOnlineMultiplayer(true);
    }

    public void SetPlayerOneHost(GameObject otherPlayer)
    {
        if (players == null)
            players = new Player[2];

        players[0] = otherPlayer.GetComponent<Player>();
        players[0].SetIndex(0);

        if (players[0] == null)
            Debug.Log("Is null");
        else
            Debug.Log("Isn't null");
    }

    public void SetPlayerTwoClient(GameObject otherPlayer)
    {
        players[1] = otherPlayer.GetComponent<Player>();
        players[1].SetIndex(1);
        players[1].gameObject.SetActive(false);

        gameMode = 2;

        UIFacade.Singleton.SetActiveOnlineMultiplayer(false);
        UIFacade.Singleton.SetActiveLocalMultiplayer(true);

        if (players[0] == null)
            Debug.Log("Is null");
        else
            Debug.Log("Isn't null");
    }

    private void SetPlayerWord()
    {
        players[playerInTurn].SetWord(UIFacade.Singleton.currentInputFieldText);

        if (turn == 0)
        {
            UIFacade.Singleton.localMultiplayerInfo.text =
                string.Format("Player 1 close your eyes, Player 2 select a word.");
        }

        if (turn == 1)
        {
            UIFacade.Singleton.SetActiveLocalMultiplayerScreen(0, false);
            UIFacade.Singleton.SetActiveLocalMultiplayerScreen(1, true);
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
        int otherPlayer = (playerInTurn + 1) % 2;

        //Played char taken from the input field
        char playedChar = UIFacade.Singleton.currentInputFieldText[0];

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

            UIFacade.Singleton.UpdateErrors(playerInTurn, players[playerInTurn].errorsCount);

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
                        UIFacade.Singleton.playerOneEmptyTexts[i].text = players[0].wordCharsArray[i].ToString();
                        break;

                    case 1:
                        UIFacade.Singleton.playerTwoEmptyTexts[i].text = players[1].wordCharsArray[i].ToString();
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

    private void GameOver(int winner)
    {
        UIFacade.Singleton.SetWinner(winner);

        UIFacade.Singleton.SetActiveLocalMultiplayerScreen(1, false);
        UIFacade.Singleton.SetActiveLocalMultiplayerScreen(2, true);
    }
}

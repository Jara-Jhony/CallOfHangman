using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int index
    {
        get; protected set;
    }

    public string word
    {
        get; protected set;
    }

    public int sucessCount
    {
        get; protected set;
    }

    public int errorsCount
    {
        get; protected set;
    }

    public char[] wordCharsArray
    {
        get; protected set;
    }
    //Word chars indexer
    public char this[int i]
    {
        get
        {
            return wordCharsArray[i];
        }
        private set
        {
            wordCharsArray[i] = value;
        }
    }

    public List<char> playedChars = new List<char>();

    public void SetIndex(int index)
    {
        this.index = index;

        switch (index)
        {
            case 0:
                Observer.Singleton.onPlayerTwoEndsTurn += Turn;
                Observer.Singleton.onPlayerOneEndsTurn += EndTurn;
                break;

            case 1:
                Observer.Singleton.onPlayerOneEndsTurn += Turn;
                Observer.Singleton.onPlayerTwoEndsTurn += EndTurn;
                break;

            default:
                return;
        }

        Debug.Log(string.Format("Player {0} created!", index));
    }

    public void SetWord(string word)
    {
        this.word = word;

        wordCharsArray = word.ToCharArray();

        switch (index)
        {
            case 0:
                for (int i = UIFacade.Singleton.playerOneEmptyTexts.Length - 1; i > wordCharsArray.Length - 1; i--)
                    UIFacade.Singleton.playerOneEmptyTexts[i].gameObject.SetActive(false);
                break;

            case 1:
                for (int i = UIFacade.Singleton.playerTwoEmptyTexts.Length - 1; i > wordCharsArray.Length - 1; i--)
                    UIFacade.Singleton.playerTwoEmptyTexts[i].gameObject.SetActive(false);
                break;

            default:
                break;
        }

        Debug.Log(string.Format("Player {0} word: {1}", index, word));
    }

    public void SetLetter(char letter)
    {
        //TODO.
    }

    public Dictionary<int, char> CheckForCharsInWord(char inputChar)
    {
        Dictionary<int, char> charsInWord = new Dictionary<int, char>();

        for (int i = 0; i < wordCharsArray.Length; i++)
        {
            if (inputChar == wordCharsArray[i])
                charsInWord.Add(i, wordCharsArray[i]);
        }

        return charsInWord;
    }

    public void IncreaseSuccessCount()
    {
        sucessCount++;
    }

    public void IncreaseErrorsCount()
    {
        errorsCount++;
    }

    private void Turn()
    {
        gameObject.SetActive(true);

        if (GameManager.Singleton.turn > 1)
        {
            switch (index)
            {
                case 0:
                    UIFacade.Singleton.playersWords[1].SetActive(true);
                    break;

                case 1:
                    UIFacade.Singleton.playersWords[0].SetActive(true);
                    break;

                default:
                    break;
            }
        }
    }

    private void EndTurn()
    {
        gameObject.SetActive(false);

        if (GameManager.Singleton.turn > 1)
        {
            switch (index)
            {
                case 0:
                    UIFacade.Singleton.playersWords[1].SetActive(false);
                    break;

                case 1:
                    UIFacade.Singleton.playersWords[0].SetActive(false);
                    break;

                default:
                    break;
            }
        }
    }
}

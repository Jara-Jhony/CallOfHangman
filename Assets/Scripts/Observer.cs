using System;
using UnityEngine;

public class Observer : MonoBehaviour {

    //Actions
    public Action onSingleplayer;
    public Action onLocalMultiplayer;
    public Action onOnlineMultiplayer;
    public Action onReadme;
    public Action onPlayerOneEndsTurn;
    public Action onPlayerTwoEndsTurn;

    //Input field Actions for singleplayer
    public Action onWordInputFieldEnterSingleplayer;
    public Action onLetterInputFieldEnterSingleplayer;
    //Input field Actions for local multiplayer
    public Action onWordInputFieldEnterLocalMultiplayer;
    public Action onLetterInputFieldEnterLocalMultiplayer;
    //Input field Actions for online multiplayer
    public Action onWordInputFieldEnterOnlineMultiplayer;
    public Action onLetterInputFieldEnterOnlineMultiplayer;

    //Singleton!
    public static Observer Singleton
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

    public void Singleplayer()
    {
        Debug.Log("Singleplayer");

        if (onSingleplayer != null)
            onSingleplayer();
    }

    public void LocalMultiplayer()
    {
        Debug.Log("LocalMultiplayer");

        if (onLocalMultiplayer != null)
            onLocalMultiplayer();
    }

    public void OnlineMultiplayer()
    {
        Debug.Log("OnlineMultiplayer");

        if (onOnlineMultiplayer != null)
            onOnlineMultiplayer();
    }

    public void Readme()
    {
        Debug.Log("Readme");

        if (onReadme != null)
            onReadme();
    }

    public void PlayerOneEndsTurn()
    {
        Debug.Log("PlayerOneEndsTurn");

        if (onPlayerOneEndsTurn != null)
            onPlayerOneEndsTurn();
    }

    public void PlayerTwoEndsTurn()
    {
        Debug.Log("PlayerTwoEndsTurn");

        if (onPlayerTwoEndsTurn != null)
            onPlayerTwoEndsTurn();
    }

    public void WordInputFieldEnterSingleplayer()
    {
        if (onWordInputFieldEnterSingleplayer != null)
            onWordInputFieldEnterSingleplayer();
    }

    public void LetterInputFieldEnterSingleplayer()
    {
        if (onLetterInputFieldEnterSingleplayer != null)
            onLetterInputFieldEnterSingleplayer();
    }

    public void WordInputFieldEnterLocalMultiplayer()
    {
        if (onWordInputFieldEnterLocalMultiplayer != null)
            onWordInputFieldEnterLocalMultiplayer();
    }

    public void LetterInputFieldEnterLocalMultiplayer()
    {
        if (onLetterInputFieldEnterLocalMultiplayer != null)
            onLetterInputFieldEnterLocalMultiplayer();
    }

    public void WordInputFieldEnterOnlineMultiplayer()
    {
        if (onWordInputFieldEnterOnlineMultiplayer != null)
            onWordInputFieldEnterOnlineMultiplayer();
    }

    public void LetterInputFieldEnterOnlineMultiplayer()
    {
        if (onLetterInputFieldEnterOnlineMultiplayer != null)
            onLetterInputFieldEnterOnlineMultiplayer();
    }
}

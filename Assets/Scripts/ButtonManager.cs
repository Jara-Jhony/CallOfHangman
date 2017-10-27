using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {

    #region Properties

    //Events
    public delegate void OnSingleplayer();
    public static event OnSingleplayer onSingleplayer;

    public delegate void OnLocalMultiplayer();
    public static event OnLocalMultiplayer onLocalMultiplayer;

    public delegate void OnOnlineMultiplayer();
    public static event OnOnlineMultiplayer onOnlineMultiplayer;

    //Singleton!
    public static ButtonManager Singleton
    {
        get; private set;
    }

    #endregion

    #region Unity Functions

    private void Awake()
    {
        if (Singleton != null)
            DestroyImmediate(gameObject);
        else
            Singleton = this;
    }

    #endregion

    #region Buttons Functions

    public void Singleplayer()
    {
        Debug.Log("SinglePlayer");

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

    #endregion
}

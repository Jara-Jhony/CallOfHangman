using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

	public Vector3 startPosition;

	public GameObject[] players = new GameObject[2];

	public GameObject[] thingsToCreat;

	public GameObject onlineGameManagerPrefab;

	public MultiplayerSetUp myMultiplayerSetUp;

	public override void OnServerConnect(NetworkConnection conn)
	{
		Debug.Log("Player was conected, "+numPlayers+" connected");
		

	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
    	var player = (GameObject)GameObject.Instantiate(playerPrefab, startPosition, Quaternion.identity);
    	NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

		if(players[0]==null)
		{
			Debug.Log(GameManager.Singleton);
			players[0] = (GameObject)player;
			GameManager.Singleton.SetHostPlayerOnline(players[0]);

		}else
		{
			players[1] = (GameObject)player;
			GameManager.Singleton.SetPlayerTwoClient(players[1]);
		}

		if(numPlayers==2)
		{
			
		}

	}

}

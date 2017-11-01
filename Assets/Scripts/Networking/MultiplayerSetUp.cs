using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerSetUp : MonoBehaviour {

	public CustomNetworkManager myNetworkManager;

	public GameObject offlineGameManager;

	public GameObject onlineGameManagerPrefab;

	public GameObject[] thigsToDestroy;
	
	void Start()
	{
		Observer.Singleton.onOnlineMultiplayer += ModeSelected;
	}

	public void ModeSelected()
	{
		GameManager.Singleton.SetupOnlineMultiplayer();
		DestroyImmediate(offlineGameManager);
		Instantiate(onlineGameManagerPrefab);
	}

	public void StartHostingMadafaca()
	{
		myNetworkManager.StopClient();
		myNetworkManager.StartHost();
		UIFacade.Singleton.SetActiveOnlineMultiplayerScreen(0,false);
		UIFacade.Singleton.SetActiveOnlineMultiplayerScreen(1,true);
	}


	public void StartBeingClient()
	{
		foreach(GameObject temp in thigsToDestroy)	
			Destroy(temp);

		myNetworkManager.StopHost();
		myNetworkManager.StopClient();
		myNetworkManager.StartClient();
		UIFacade.Singleton.SetActiveOnlineMultiplayerScreen(0,false);
		UIFacade.Singleton.SetActiveOnlineMultiplayerScreen(1,true);
		
	}


}

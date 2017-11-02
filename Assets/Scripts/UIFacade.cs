using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFacade : MonoBehaviour {

    [SerializeField]
    private GameObject mainMenu;

    [Space(10f)]

    public Singleplayer singleplayer;
    public LocalMultiplayer localMultiplayer;
    public OnlineMultiplayer onlineMultiplayer;

    //Singleton!
    public static UIFacade Singleton
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

    public void SetActiveMainMenu(bool state)
    {
        mainMenu.SetActive(state);
    }

    public void Done()
    {
        SceneManager.LoadScene(0);
    }
}

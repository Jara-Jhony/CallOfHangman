public class OnlineMultiplayer : UserInterface {

    public void SetActiveOnlineMultiplayer(bool state)
    {
        gameModeScreen.SetActive(state);
    }

    public void SetActiveOnlineMultiplayerScreen(int screen, bool state)
    {
        if (screen < 0 || screen > screens.Length - 1)
            return;

        screens[screen].SetActive(state);
    }
}

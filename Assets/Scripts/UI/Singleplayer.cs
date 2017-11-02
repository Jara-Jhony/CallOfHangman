public class Singleplayer : UserInterface {

    public void SetActiveSingleplayer(bool state)
    {
        gameModeScreen.SetActive(state);
    }

    public void SetActiveSingleplayerScreen(int screen, bool state)
    {
        if (screen < 0 || screen > screens.Length - 1)
            return;

        screens[screen].SetActive(state);
    }
}

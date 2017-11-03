using UnityEngine.UI;

public class Singleplayer : UserInterface {

    public Button letterButton;

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

    public void SetActiveLetterSection(bool state)
    {
        letterButton.interactable = state;
        letterInputField.interactable = state;
    }
}

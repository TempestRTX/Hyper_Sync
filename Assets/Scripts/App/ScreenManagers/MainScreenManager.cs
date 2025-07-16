using UnityEngine;
using UnityEngine.UI;

public class MainScreenManager : ScreenManager
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button exitGameButton;

    protected override void InitScreen()
    {
        base.InitScreen();
        startGameButton.onClick.AddListener(StartGame);
        exitGameButton.onClick.AddListener(QuitGame);
    }

    protected override void DeactivateScreen()
    {
        base.DeactivateScreen();
        startGameButton.onClick.RemoveAllListeners();
        exitGameButton.onClick.RemoveAllListeners();
    }

    public void StartGame()
    {
        gameManager.StartGame();
    }

    public void QuitGame()
    {
        gameManager.QuitApplication();
    }
}

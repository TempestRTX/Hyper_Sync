using UnityEngine;
using UnityEngine.UI;

public class MainScreenManager : ScreenManager
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button exitGameButton;

    protected override void Init()
    {
        base.Init();
        startGameButton.onClick.AddListener(StartGame);
        exitGameButton.onClick.AddListener(QuitGame);
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

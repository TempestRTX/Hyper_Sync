using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenManager : ScreenManager
{
    [SerializeField] private TextMeshProUGUI Scoretext;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button HomeButton;

    protected override void InitScreen()
    {
        base.InitScreen();
        RestartButton.onClick.AddListener(RestartGame);
        HomeButton.onClick.AddListener(HomeGame);
        Scoretext.text = gameManager.GetScore().ToString();
    }

    protected override void DeactivateScreen()
    {
        base.DeactivateScreen();
        RestartButton.onClick.RemoveAllListeners();
        HomeButton.onClick.RemoveAllListeners();
    }

    public void RestartGame()
    {
        gameManager.RestartGame();
    }

    public void HomeGame()
    {
        gameManager.GotoMainMenu();
    }
    
}

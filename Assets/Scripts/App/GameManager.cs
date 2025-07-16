using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : GenericSingleton<GameManager>
{
    private SceneManager _sceneManager;
    private void Start()
    {
        InitGameManager();
    }

    private void InitGameManager()
    {
        PlayerScore.HighScore= PlayerPrefs.GetInt("HighScore", 0);
    }

    #region Game Workflow

    private void ChangeGameState(GameState.AppState state, GameState.UserAction action)
    {
        switch (state)
        {
            case GameState.AppState.MainMenu:
                ManageUserActionfromMainMenu(action);
                break;
            case GameState.AppState.Gameplay:
                ManageUserActionfromGameplay(action);
                break;
            case GameState.AppState.GameOver:
                ManageUserActionfromGameOver(action);
                break;
        }
    }

    private void ManageUserActionfromGameplay(GameState.UserAction action)
    {
        if (action==GameState.UserAction.GameOver)
        {
            LoadScene(GameState.AppState.GameOver.ToString());
        }
    }

    private void ManageUserActionfromGameOver(GameState.UserAction action)
    {
        if (action==GameState.UserAction.RestartGame)
        {
            LoadScene(GameState.AppState.Gameplay.ToString());
        }
        else if (action == GameState.UserAction.GoToMainMenu)
        {
            LoadScene(GameState.AppState.MainMenu.ToString());
        }
    }
    
    private void ManageUserActionfromMainMenu(GameState.UserAction action)
    {
        if (action==GameState.UserAction.StartGame)
        {
            LoadScene(GameState.AppState.Gameplay.ToString());
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void StartGame()
    {
        ChangeGameState(GameState.AppState.MainMenu, GameState.UserAction.StartGame);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    [ContextMenu("Load Game Over")]
    public void GameOver()
    {
        ChangeGameState(GameState.AppState.Gameplay, GameState.UserAction.GameOver);
    }

    public void GotoMainMenu()
    {
        ChangeGameState(GameState.AppState.GameOver, GameState.UserAction.GoToMainMenu);
    }

    public void RestartGame()
    {
        ChangeGameState(GameState.AppState.GameOver, GameState.UserAction.RestartGame);

    }

    #endregion
    
    #region Score

    public GameState.Score PlayerScore = new GameState.Score();

    public int GetHighScore()
    {
        return PlayerScore.HighScore;
    }

    public int GetScore()
    {
        return PlayerScore.SessionScore;
    }

    public void ResetScore()
    {
        PlayerScore.SessionScore = 0;
    }

    public void SetHighScore(int score)
    {
        PlayerScore.HighScore = score;
        PlayerPrefs.SetInt("HighScore", PlayerScore.HighScore);
        PlayerPrefs.Save();
    }

    public void AddScore()
    {
        PlayerScore.SessionScore +=1 ;
        if (PlayerScore.SessionScore > PlayerScore.HighScore)
        {
            SetHighScore(PlayerScore.SessionScore);
        }
    }


    #endregion
}

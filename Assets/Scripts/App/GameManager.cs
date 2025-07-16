using System;
using UnityEngine;

public class GameManager : GenericSingleton<GameManager>
{
    private void Start()
    {
        InitGameManager();
    }

    private void InitGameManager()
    {
        PlayerScore.HighScore= PlayerPrefs.GetInt("HighScore", 0);
    }

    #region Game Workflow

    private void ChangeGameState(GameState state)
    {
       
    }
    public void StartGame()
    {
        
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        
    }

    public void GotoMainMenu()
    {
        
    }

    public void RestartGame()
    {
        
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

    public void SetHighScore(int score)
    {
        PlayerScore.HighScore = score;
        PlayerPrefs.SetInt("HighScore", PlayerScore.HighScore);
        PlayerPrefs.Save();
    }

    public void SetScore(int score)
    {
        PlayerScore.SessionScore = score;
        if (score > PlayerScore.HighScore)
        {
            SetHighScore(score);
        }
    }


    #endregion
}

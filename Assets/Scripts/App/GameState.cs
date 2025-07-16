using UnityEngine;

public class GameState 
{
    public enum AppState
    {
        MainMenu,
        Gameplay,
        GameOver
    }
    
    public enum UserAction
    {
        StartGame,
        QuitGame,
        RestartGame,
        GoToMainMenu,
        BackButton,
        GameOver
    }
    
    //Score
    public class Score
    {
        public int SessionScore;
        public int HighScore;
    }
}

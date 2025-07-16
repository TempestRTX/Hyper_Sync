using UnityEngine;

public class GameState 
{
    public enum AppState
    {
        MainMenu,
        Gameplay,
        GameOver
    }
    
    //Score
    public class Score
    {
        public int SessionScore;
        public int HighScore;
    }
}

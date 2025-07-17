using UnityEngine;

public class GameState 
{
    public enum AppState
    {
        MainMenu,
        Gameplay,
        GameOver
    }

    public enum ObjectTags
    {
        Player,
        Ground,
        Collectible,
        Obstacle
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
    
    //Networking
    public static class GameEvents
    {
        public const string PlayerMoveDelta = "PlayerMoveDelta";
        public const string PlayerJump = "PlayerJump";
        public const string PlayerHitObstacle = "PlayerHitObstacle";
        public const string PlayerCollectedOrb = "PlayerCollectedOrb";
    }

}

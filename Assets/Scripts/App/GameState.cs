using System.Collections.Generic;
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
    
    
    public enum PlayerActionType { Jump, CollectOrb, LaneChange,HitObstacle }
    public struct PlayerActionEvent
    {
        public float timestamp;
        public PlayerActionType type;
        public Vector3 position;
        public int param;
    }
    
    public class EventQueue
    {
        private Queue<PlayerActionEvent> actions = new Queue<PlayerActionEvent>();
        private float networkDelay = 0.1f;

        public void Enqueue(PlayerActionEvent action)
        {
            actions.Enqueue(action);
        }

        public PlayerActionEvent? DequeueIfReady(float currentTime)
        {
            if (actions.Count == 0) return null;
            if (currentTime - actions.Peek().timestamp >= networkDelay)
                return actions.Dequeue();
            return null;
        }

        public void Clear() => actions.Clear();
    }
    
    public enum SpawnObjectType { Collectable, Obstacle }

    public struct SpawnObjectEvent
    {
        public Vector3 position;
        public SpawnObjectType type;
        public float timestamp;
    }
    public class ObjectEventQueue
    {
        private Queue<SpawnObjectEvent> actions = new Queue<SpawnObjectEvent>();
        private float networkDelay = 0.1f;

        public void Enqueue(SpawnObjectEvent action)
        {
            actions.Enqueue(action);
        }

        public SpawnObjectEvent DequeueIfReady()
        {
            if (actions.Count == 0)
                return new SpawnObjectEvent();
            return actions.Dequeue();
        }


        public void Clear() => actions.Clear();
    }
}

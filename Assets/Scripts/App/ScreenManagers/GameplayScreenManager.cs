using UnityEngine;

public class GameplayScreenManager : ScreenManager
{
    [SerializeField] PlayerController player;
    [SerializeField]  GhostController ghost;
    private GameState.EventQueue eventQueue = new GameState.EventQueue();

      protected override void InitScreen()
      {
          player.OnActionEvent += action => eventQueue.Enqueue(action);
          ghost.eventQueue = eventQueue;
          // Init obstacles, orbs, pooling, UI, etc.
      }
    
}

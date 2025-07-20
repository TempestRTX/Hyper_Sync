using UnityEngine;

public class GameplayScreenManager : ScreenManager
{
    [SerializeField] PlayerController player;
    [SerializeField]  GhostController ghost;
    
    [SerializeField]  ObjectSpawner spawner;
    [SerializeField]  GhostObjectSpawner ghostObjectSpawner;
    private GameState.EventQueue eventQueue = new GameState.EventQueue();
    private GameState.ObjectEventQueue spawnEvent = new GameState.ObjectEventQueue();

      protected override void InitScreen()
      {
          player.OnActionEvent += action => eventQueue.Enqueue(action);
          ghost.eventQueue = eventQueue;
          spawner.OnActionEvent += HandleSpawnAction;
          ghostObjectSpawner.eventQueue = spawnEvent;
         
      }
      
      void HandleSpawnAction(GameState.SpawnObjectEvent action)
      {
          spawnEvent.Enqueue(action); 
         
      }

      
    
}

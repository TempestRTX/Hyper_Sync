using UnityEngine;

public class GhostObjectSpawner : MonoBehaviour
{
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] public GameState.ObjectEventQueue eventQueue= new GameState.ObjectEventQueue();
    [SerializeField] private Transform player; // Reference to the player

    private float spawnTimer = 0f;

    void Update()
    {
        if (spawnTimer < spawnDelay)
        {
            spawnTimer += Time.deltaTime;
            return;
        }

        var spawnEvent = eventQueue.DequeueIfReady(Time.time);
        if (spawnEvent.HasValue)
        {
            ProcessGhostSpawnEvent(spawnEvent.Value);
        }
    }

    void ProcessGhostSpawnEvent(GameState.SpawnObjectEvent spawnEvent)
    {
        if (player == null)
        {
            Debug.LogError("Player Transform not set on GhostObjectSpawner!");
            return;
        }

        
        Vector3 spawnWorldPos = new Vector3(
            player.position.x + spawnEvent.position.x,
            player.position.y + spawnEvent.position.y,
            spawnEvent.position.z 
        );

        switch (spawnEvent.type)
        {
            case GameState.SpawnObjectType.Obstacle:
                GhostObjectPooler.Instance.SpawnFromPool(GameState.ObjectTags.Obstacle.ToString(), spawnWorldPos, Quaternion.identity);
                break;
            case GameState.SpawnObjectType.Collectable:
                GhostObjectPooler.Instance.SpawnFromPool(GameState.ObjectTags.Collectible.ToString(), spawnWorldPos, Quaternion.identity);
                break;
        }
    }
}
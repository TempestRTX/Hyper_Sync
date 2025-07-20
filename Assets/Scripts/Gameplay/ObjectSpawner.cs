using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]  float spawnDistance = 20f;
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] Transform player;
    private float spawnTimer = 0f;
    
    public Action<GameState.SpawnObjectEvent> OnActionEvent;
    

    void Start() {
        lastSpawnZ = player.position.z + spawnDistance;
    }

    void Update()
    {
        // Wait before starting obstacle spawn
        if (spawnTimer < spawnDelay)
        {
            spawnTimer += Time.deltaTime;
            return;
        }

        // Spawn when player is close to the next spawn point
        if (player.position.z + spawnDistance > lastSpawnZ)
        {
            SpawnObstacleOrCollectible();
            lastSpawnZ += 10f;
        }
    }

    private float lastSpawnZ = 0f;

    void SpawnObstacleOrCollectible()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-2f, 2f), 0.5f, lastSpawnZ);

        if (Random.value > 0.5f)
        {
            Objectpooler.Instance.SpawnFromPool(GameState.ObjectTags.Obstacle.ToString(), spawnPos, Quaternion.identity);
            OnActionEvent?.Invoke(new GameState.SpawnObjectEvent()
            {
                timestamp = Time.time,
                type = GameState.SpawnObjectType.Obstacle,
                position = transform.position
            });
        }
        else
        {
            spawnPos.y = 1f; // slightly higher for collectible
            Objectpooler.Instance.SpawnFromPool(GameState.ObjectTags.Collectible.ToString(), spawnPos, Quaternion.identity);
            OnActionEvent?.Invoke(new GameState.SpawnObjectEvent()
            {
                timestamp = Time.time,
                type = GameState.SpawnObjectType.Obstacle,
                position = transform.position
            });
        }
    }
}
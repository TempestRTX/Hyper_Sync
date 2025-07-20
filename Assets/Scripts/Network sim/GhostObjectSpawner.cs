using UnityEngine;
using System.Collections;

public class GhostObjectSpawner : MonoBehaviour
{
    [SerializeField] private int batchSize = 10;
    public GameState.ObjectEventQueue eventQueue;
    [SerializeField] private Transform player;

    private void OnEnable() {
        StartCoroutine(ProcessEventsCoroutine());
    }

    private IEnumerator ProcessEventsCoroutine()
    {
        while (true)
        {
            int processed = 0;
            while (processed < batchSize && eventQueue != null)
            {
                var spawnEvent = eventQueue.DequeueIfReady();
                if (spawnEvent.timestamp==0f) break;
                ProcessGhostSpawnEvent(spawnEvent);
                processed++;
            }
            yield return null;  // Defer remaining events to next frame
        }
    }

    private void ProcessGhostSpawnEvent(GameState.SpawnObjectEvent spawnEvent)
    {
        if (player == null)
        {
            Debug.LogError("GhostObjectSpawner: Player Transform not set!");
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
                GhostObjectPooler.Instance.SpawnFromPool(GameState.ObjectTags.Obstacle.ToString()+"Ghost", spawnWorldPos, Quaternion.identity);
                break;
            case GameState.SpawnObjectType.Collectable:
                GhostObjectPooler.Instance.SpawnFromPool("CollectibleGhost", spawnWorldPos, Quaternion.identity);
                break;
            default:
                Debug.LogWarning("GhostObjectSpawner: Unknown SpawnObjectType: " + spawnEvent.type);
                break;
        }
    }
}

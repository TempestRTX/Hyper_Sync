using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]  float spawnDistance = 20f;
    [SerializeField] Transform player;

    void Update()
    {
        if (player.position.z + spawnDistance > lastSpawnZ)
        {
            SpawnObstacleOrCollectible();
            lastSpawnZ += 10f; // next spawn point
        }
    }

    private float lastSpawnZ = 0f;

    void SpawnObstacleOrCollectible()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-2f, 2f), 0.5f, lastSpawnZ);

        if (Random.value > 0.5f)
        {
            Objectpooler.Instance.SpawnFromPool("Obstacle", spawnPos, Quaternion.identity);
        }
        else
        {
            spawnPos.y = 1f; // slightly higher for collectible
            Objectpooler.Instance.SpawnFromPool("Collectible", spawnPos, Quaternion.identity);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class GhostGroundSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float tileLength = 10f;
    [SerializeField] private int tilesOnScreen = 6;
    [SerializeField] private float safeZone = 15f;

    private float spawnZ = 0f;
    private float lastPlayerZ;
    private Queue<GameObject> activeTiles = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < tilesOnScreen; i++)
        {
            SpawnGround();
        }

        lastPlayerZ = player.position.z;
    }

    void Update()
    {
        if (player.position.z - safeZone > spawnZ - tilesOnScreen * tileLength)
        {
            RecycleTile();
            SpawnGround();
        }
    }

    void SpawnGround()
    {
        Vector3 spawnPos = new Vector3(player.position.x, player.position.y, spawnZ);
        GameObject groundTile = GhostObjectPooler.Instance.SpawnFromPool("GroundGhost", spawnPos, Quaternion.identity);
        activeTiles.Enqueue(groundTile);
        spawnZ += tileLength;
    }

    void RecycleTile()
    {
        if (activeTiles.Count == 0) return;

        GameObject oldTile = activeTiles.Dequeue();
        oldTile.SetActive(false);
    }
}

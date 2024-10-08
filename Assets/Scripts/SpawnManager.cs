using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsToSpawn;   // Array of different prefabs to spawn
    [SerializeField] private Collider2D spawnAreaCollider;  // Reference to the collider that defines the spawn area
    [SerializeField] private float spawnInterval = 2f;      // Time between each spawn
    [SerializeField] private int maxInstances = 10;         // Maximum number of instances to spawn
    private int currentSpawned = 0;       // Counter for current number of spawned objects

    [SerializeField] private Transform parentTransform;     // Parent object to instantiate under

    void Start()
    {
        // Start spawning prefabs repeatedly at regular intervals
        InvokeRepeating("SpawnRandomPrefab", 0f, spawnInterval);
    }

    void SpawnRandomPrefab()
    {
        if (GameManager.instance.objecctsCanMove)
        {
            // Stop spawning if we've reached the max instances
            if (currentSpawned >= maxInstances)
            {
                CancelInvoke("SpawnRandomPrefab");
                return;
            }

            // Choose a random prefab to spawn from the array
            GameObject prefabToSpawn = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];

            // Generate a random spawn position within the collider's bounds
            Vector2 spawnPosition = GetRandomPointInCollider(spawnAreaCollider);

            // Spawn the chosen prefab at the random position
            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity, parentTransform);

            // Increment the number of spawned objects
            currentSpawned++;
        }
    }

    // Get a random point within the bounds of the collider
    Vector2 GetRandomPointInCollider(Collider2D collider)
    {
        Bounds bounds = collider.bounds;

        // Generate random x and y within the bounds
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        // Return the random position inside the collider's bounds
        return new Vector2(randomX, randomY);
    }
}

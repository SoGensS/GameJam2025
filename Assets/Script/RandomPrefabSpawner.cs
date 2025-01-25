using UnityEngine;

public class RandomPrefabSpawner : MonoBehaviour
{
    public GameObject[] prefabs; // Array of prefabs to spawn
    public float spawnInterval = 2f; // Time between spawns
    public float destroyDelay = 5f; // Time after which the spawned prefab is destroyed

    private float timer;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnRandomPrefab();
            timer = spawnInterval;
        }
    }

    void SpawnRandomPrefab()
    {
        if (prefabs.Length == 0)
        {
            Debug.LogWarning("No prefabs assigned to spawn.");
            return;
        }

        // Choose a random prefab from the array
        GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

        // Generate a random rotation
        Quaternion spawnRotation = Quaternion.Euler(
            Random.Range(0, 360),
            Random.Range(0, 360),
            Random.Range(0, 360)
        );

        // Spawn the prefab at the exact position of this GameObject
        GameObject spawnedPrefab = Instantiate(prefabToSpawn, transform.position, spawnRotation);

        // Destroy the spawned prefab after the specified delay
        Destroy(spawnedPrefab, destroyDelay);
    }
}
using UnityEngine;

public class RandomPrefabSpawner : MonoBehaviour
{
    public GameObject[] prefabs; // Array of prefabs to spawn
    public AudioClip[] sounds; // Array of sounds to play
    public float spawnInterval = 2f; // Time between spawns
    public float destroyDelay = 5f; // Time after which the spawned prefab is destroyed
    public float soundDelay = 3f; // Time after which the sound is played

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

        // Play a random sound after the specified delay
        if (sounds.Length > 0)
        {
            StartCoroutine(PlayRandomSoundAfterDelay(spawnedPrefab, soundDelay));
        }

        // Destroy the spawned prefab after the specified delay
        Destroy(spawnedPrefab, destroyDelay);
    }

    private System.Collections.IEnumerator PlayRandomSoundAfterDelay(GameObject spawnedPrefab, float delay)
    {   
        if(FindFirstObjectByType<Player>()){
            yield return new WaitForSeconds(delay);

            // Choose a random sound from the array
            AudioClip soundToPlay = sounds[Random.Range(0, sounds.Length)];

            // Get the AudioSource component from the spawned prefab
            AudioSource audioSource = spawnedPrefab.GetComponent<AudioSource>();

            // If the prefab doesn't have an AudioSource, add one
            if (audioSource == null)
            {
                audioSource = spawnedPrefab.AddComponent<AudioSource>();
            }

            // Play the sound
            audioSource.PlayOneShot(soundToPlay,0.3f);
        }
        
    }
}
using UnityEngine;

public class RandomPrefabSpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public AudioClip[] sounds;
    public float spawnInterval = 2f;
    public float destroyDelay = 5f;
    public float soundDelay = 3f;

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

        GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

        Quaternion spawnRotation = Quaternion.Euler(
            Random.Range(0, 360),
            Random.Range(0, 360),
            Random.Range(0, 360)
        );

        GameObject spawnedPrefab = Instantiate(prefabToSpawn, transform.position, spawnRotation);
        spawnedPrefab.name = prefabToSpawn.name;

        if (sounds.Length > 0)
        {
            StartCoroutine(PlayRandomSoundAfterDelay(spawnedPrefab, soundDelay));
        }

        Destroy(spawnedPrefab, destroyDelay);
    }

    private System.Collections.IEnumerator PlayRandomSoundAfterDelay(GameObject spawnedPrefab, float delay)
    {
        if (FindFirstObjectByType<Player>())
        {
            yield return new WaitForSeconds(delay);

            AudioClip soundToPlay = sounds[Random.Range(0, sounds.Length)];

            AudioSource audioSource = spawnedPrefab.GetComponent<AudioSource>();

            if (audioSource == null)
            {
                audioSource = spawnedPrefab.AddComponent<AudioSource>();
            }

            audioSource.PlayOneShot(soundToPlay, 0.3f);
        }
    }
}
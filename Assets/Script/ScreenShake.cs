using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    public bool start = true; // Toggle to start/stop the shake
    public float shakeMagnitude = 0.1f; // Controls the intensity of the shake

    void Update()
    {
        if (start)
        {
            start = false; // Prevent restarting the coroutine
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position; // Store the initial position of the camera

        while (true) // Infinite loop for continuous shaking
        {
            // Generate a random offset within a unit sphere and scale it by shakeMagnitude
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;

            // Apply the offset to the camera's position
            transform.position = startPosition + randomOffset;

            yield return null; // Wait until the next frame
        }
    }

    /// <summary>
    /// Stops the shaking and resets the camera to its original position.
    /// </summary>
    public void StopShake()
    {
        StopAllCoroutines(); // Stop the shaking coroutine
        transform.position = Vector3.zero; // Reset the camera position
    }
}
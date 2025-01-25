using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineSceneChanger : MonoBehaviour
{
    private PlayableDirector director;

    private void Start()
    {
        // Get the PlayableDirector component
        director = GetComponent<PlayableDirector>();

        // Subscribe to the Timeline's stopped event
        director.stopped += OnTimelineFinished;
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        // Get the current scene's build index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Calculate the next scene's build index
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is within the range of the build settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Load the next scene
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene available. Reached the end of the build settings.");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks
        if (director != null)
        {
            director.stopped -= OnTimelineFinished;
        }
    }
}
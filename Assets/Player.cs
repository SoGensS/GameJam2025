using UnityEngine;
using UnityEngine.Playables;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public float shrinkSpeed = 0.1f; // Speed of shrinking
    public float restoreSpeed = 0.1f; // Speed of restoring size
    public float minScale = 0.1f; // Minimum scale limit
    public float holdDuration = 1f; // Max time to hold before forcing size reset
    public float cooldownTime = 3f; // Cooldown time in seconds

    private Vector3 originalScale; // Store the original scale
    private Rigidbody rb;
    private Vector2 moveInput;
    private Transform cameraTransform;

    private float holdTimer = 0f; // Timer for holding spacebar
    private float cooldownTimer = 0f; // Timer for cooldown
    private bool isCooldown = false; // Whether the cooldown is active
    public UIManager uiManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Lock rotation so it doesn't tip over
        rb.freezeRotation = true;

        // Get the parent's transform (assuming the camera is the parent)
        cameraTransform = transform.parent;

        // Store the original scale of the player
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Get input from horizontal (X) and vertical (Y) axes
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Cooldown management
        if (isCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownTime)
            {
                isCooldown = false;
                cooldownTimer = 0f;
            }
        }

        // Handle shrinking logic if cooldown is not active
        if (!isCooldown)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                holdTimer += Time.deltaTime;

                if (holdTimer > holdDuration)
                {
                    // Force size reset and trigger cooldown
                    ForceSizeReset();
                }
                else
                {
                    ShrinkPlayer();
                }
            }
            else
            {
                holdTimer = 0f; // Reset hold timer
                RestoreSize();
            }
        }
        else
        {
            // Restore size during cooldown
            RestoreSize();
        }
    }

    void FixedUpdate()
    {
        // Calculate movement vector in the X and Y axes
        Vector3 moveVector = new Vector3(moveInput.x, moveInput.y, 0f) * moveSpeed * Time.fixedDeltaTime;

        // Apply rotation of the camera to the player's movement
        Quaternion cameraRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        Vector3 rotatedMoveVector = cameraRotation * moveVector;

        // Apply movement to the Rigidbody
        rb.MovePosition(transform.position + rotatedMoveVector);

        // Update the player's rotation to match the camera's rotation
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
    }

    private void ShrinkPlayer()
    {
        // Gradually reduce the scale of the player
        if (transform.localScale.x > minScale)
        {
            transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;
        }
    }

    private void RestoreSize()
    {
        // Gradually restore the scale of the player
        if (transform.localScale.x < originalScale.x)
        {
            transform.localScale += Vector3.one * restoreSpeed * Time.deltaTime;

            // Clamp the scale to ensure it doesn't exceed the original size
            transform.localScale = Vector3.Min(transform.localScale, originalScale);
        }
    }

    private void ForceSizeReset()
    {
        // Force reset the scale to original size
        transform.localScale = originalScale;

        // Start cooldown
        isCooldown = true;

        // Reset hold timer
        holdTimer = 0f;
    }

    void StopAllPlayingTimelines()
    {
        // Find all PlayableDirector components in the scene
        PlayableDirector[] directors = FindObjectsOfType<PlayableDirector>();

        foreach (PlayableDirector director in directors)
        {
            // Check if the timeline is currently playing
            if (director.state == PlayState.Playing)
            {
                // Stop the timeline
                director.Pause();
                Debug.Log($"Stopped timeline on object: {director.gameObject.name}");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            StopAllPlayingTimelines();
            uiManager.Death(collision.gameObject);
            Destroy(gameObject);
            //Effect Play
        }
    }
}

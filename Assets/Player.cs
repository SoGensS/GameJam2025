using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement

    private Rigidbody rb;
    private Vector2 moveInput;
    private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Lock rotation so it doesn't tip over
        rb.freezeRotation = true;

        // Get the parent's transform (assuming the camera is the parent)
        cameraTransform = transform.parent;
    }

    void Update()
    {
        // Get input from horizontal (X) and vertical (Y) axes
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}

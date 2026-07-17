using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;

    private AudioSource footstepAudio;
    private bool isMoving;

    private float minX, maxX, minY, maxY;
    private Camera mainCamera;

    [Header("Footstep Settings")]
    public float stepInterval = 0.4f;
    private float stepTimer;

    [Header("Spirit Visual")]
    public Transform spiritVisual;

    [Header("Werewolf Avoidance")]
    public Transform werewolf; // Assign in inspector
    public float werewolfAvoidDistance = 1.5f;
    public float bounceStrength = 3f;

    void Start()
    {
        footstepAudio = GetComponent<AudioSource>();
        mainCamera = Camera.main;

        float vertExtent = mainCamera.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        minX = mainCamera.transform.position.x - horzExtent;
        maxX = mainCamera.transform.position.x + horzExtent;
        minY = mainCamera.transform.position.y - vertExtent;
        maxY = mainCamera.transform.position.y + vertExtent;

        stepTimer = stepInterval;
    }

    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        Vector3 moveDirection = (Vector3)(moveInput * moveSpeed * Time.deltaTime);

        // Check werewolf distance
        if (werewolf != null)
        {
            float distance = Vector2.Distance(transform.position, werewolf.position);
            if (distance < werewolfAvoidDistance)
            {
                // Bounce away from werewolf
                Vector2 bounceDir = (transform.position - werewolf.position).normalized;
                moveDirection += (Vector3)(bounceDir * bounceStrength * Time.deltaTime);
            }
        }

        // Apply movement
        Vector3 newPosition = transform.position + moveDirection;

        // Clamp to camera bounds
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        transform.position = newPosition;

        // Is moving?
        isMoving = moveInput != Vector2.zero;

        // Footsteps
        if (isMoving)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                footstepAudio.pitch = Random.Range(0.95f, 1.05f);
                footstepAudio.PlayOneShot(footstepAudio.clip);
                stepTimer = stepInterval;
            }

            // Flip spirit visual
            if (moveInput.x > 0)
                spiritVisual.localScale = new Vector3(1, 1, 1);
            else if (moveInput.x < 0)
                spiritVisual.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            stepTimer = stepInterval;
        }
    }
}

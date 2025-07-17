using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float speedIncreaseRate = 0.2f;
    [SerializeField] private Rigidbody rb;

    [Header("Lane Settings")]
    [SerializeField] private float laneDistance = 2f;
    [SerializeField] private float laneChangeSpeed = 10f;
    private int currentLane = 1; // 0 = Left, 1 = Center, 2 = Right
    private int totalLanes = 3;
    private Vector3 targetPosition;

    [Header("Swipe Settings")]
    private Vector2 touchStart;
    private Vector2 touchEnd;
    private float swipeThreshold = 50f;

    [Header("State")]
    [SerializeField] private bool isGrounded = true;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        targetPosition = transform.position;
    }

    void Update()
    {
        // Auto-forward movement
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Handle jump
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && isGrounded)
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0 && isGrounded)
#endif
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Swipe movement
        HandleSwipe();

        // Smooth horizontal lane movement
        Vector3 newPosition = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, laneChangeSpeed * Time.deltaTime);

        // Gradually increase speed
        speed += speedIncreaseRate * Time.deltaTime;
    }

    private void HandleSwipe()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            touchEnd = Input.mousePosition;
            DetectSwipe();
        }
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStart = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                touchEnd = touch.position;
                DetectSwipe();
            }
        }
#endif
    }

    private void DetectSwipe()
    {
        float deltaX = touchEnd.x - touchStart.x;

        if (Mathf.Abs(deltaX) > swipeThreshold)
        {
            if (deltaX > 0)
                ChangeLane(1); // Swipe right
            else
                ChangeLane(-1); // Swipe left
        }
    }

    private void ChangeLane(int direction)
    {
        currentLane += direction;
        currentLane = Mathf.Clamp(currentLane, 0, totalLanes - 1);
        float xPos = (currentLane - 1) * laneDistance;
        targetPosition = new Vector3(xPos, transform.position.y, transform.position.z);
    }

    private void OnCollisionEnter(Collision other)
    {
        string tag = other.gameObject.tag;

        if (tag == GameState.ObjectTags.Ground.ToString())
        {
            isGrounded = true;
        }
        else if (tag == GameState.ObjectTags.Collectible.ToString())
        {
            gameManager.AddScore();
            other.gameObject.SetActive(false);
        }
        else if (tag == GameState.ObjectTags.Obstacle.ToString())
        {
            gameManager.GameOver();
        }
    }
}

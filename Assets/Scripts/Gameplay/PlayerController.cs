using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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
    private Vector3 lastPosition;

    [Header("Swipe Settings")]
    private Vector2 touchStart;
    private Vector2 touchEnd;
    private float swipeThreshold = 50f;

    [Header("State")]
    [SerializeField] private bool isGrounded = true;
    
    public Action<GameState.PlayerActionEvent> OnActionEvent;

    private GameManager gameManager;
    
    [SerializeField] private GameObject MainCamera;
    [SerializeField] private GameObject GhostCamera;
    [SerializeField] private GameObject RippleCamera;
    [SerializeField] private GameObject MainCanvas;
    
    //Fx
    [SerializeField] PostProcessVolume postProcessVolume;
    private ChromaticAberration chromaticAberration;
    [SerializeField] TextureParameter chromaticAberrationParameter;
    
    [SerializeField] CameraShake cameraShake;

    private bool IsGameEventRunning;

    private void Start()
    {
        gameManager = GameManager.Instance;
        targetPosition = transform.position;
        postProcessVolume.profile.TryGetSettings(out chromaticAberration);
    }

  

    void Update()
    {
        if (!IsGameEventRunning)
        {
            // Auto-forward movement
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            // Delta movement event
            Vector3 delta = transform.position - lastPosition;
            if (delta.sqrMagnitude > 0f)
                EventManager.Instance.TriggerEvent(GameState.GameEvents.PlayerMoveDelta, delta);
            lastPosition = transform.position;

            // Handle jump
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) && isGrounded)
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0 && isGrounded)
#endif
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                OnActionEvent?.Invoke(new GameState.PlayerActionEvent
                {
                    timestamp = Time.time,
                    type = GameState.PlayerActionType.Jump,
                    position = transform.position
                });
                EventManager.Instance.TriggerEvent(GameState.GameEvents.PlayerJump);
            }

            // Swipe movement
            HandleSwipe();

            // Smooth horizontal lane movement
            Vector3 newPosition = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, laneChangeSpeed * Time.deltaTime);

            // Gradually increase speed
            speed += speedIncreaseRate * Time.deltaTime;
        }
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
        int prevLane = currentLane;
        currentLane = Mathf.Clamp(currentLane + direction, 0, totalLanes - 1);

        if (prevLane != currentLane)
        {
            float xPos = (currentLane - 1) * laneDistance;
            targetPosition = new Vector3(xPos, transform.position.y, transform.position.z);

            // Sync lane change event
            OnActionEvent?.Invoke(new GameState.PlayerActionEvent
            {
                timestamp = Time.time,
                type = GameState.PlayerActionType.LaneChange,
                position = targetPosition, // The actual new x-position
                param = currentLane        // Send the new lane index!
            });
            //EventManager.Instance.TriggerEvent(GameState.GameEvents.PlayerLaneChange, currentLane);
        }
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
            OnActionEvent?.Invoke(new GameState.PlayerActionEvent
            {
                timestamp = Time.time,
                type = GameState.PlayerActionType.CollectOrb,
                position = transform.position
            });
            gameManager.AddScore();
            //other.gameObject.SetActive(false);
            EventManager.Instance.TriggerEvent(GameState.GameEvents.PlayerCollectedOrb, other.transform.position);
        }
        else if (tag == GameState.ObjectTags.Obstacle.ToString())
        {
            OnActionEvent?.Invoke(new GameState.PlayerActionEvent
            {
                timestamp = Time.time,
                type = GameState.PlayerActionType.HitObstacle,
                position = transform.position
            });
            EventManager.Instance.TriggerEvent(GameState.GameEvents.PlayerHitObstacle, other.transform.position);
            IsGameEventRunning = true;
            StartCoroutine(ChangeIntensity(2f));
        }
    }
    
    private void ChromaticAberrationONOFF(bool isOn)
    {
        if (isOn)
        {
            chromaticAberration.active = true;
            
        }
        else
        {
            chromaticAberration.active = false;
        }
    }

     private IEnumerator ChangeIntensity(float intensity)
    {
       
        while (chromaticAberration.intensity.value<intensity)
        {
            yield return new WaitForEndOfFrame();
            var intess = chromaticAberration.intensity.value + 0.1f;
            chromaticAberration.intensity.value = intess;
            
        }
        yield return new WaitForSeconds(2f);
        cameraShake.TriggerShake(1,1);
        yield return new WaitForSeconds(1.3f);
        //Disable player cams and show ripple fx
        MainCamera.SetActive(false);
        RippleCamera.SetActive(true);
        GhostCamera.SetActive(false);
        MainCanvas.SetActive(false);
        yield return new WaitForSeconds(1.3f);
        gameManager.GameOver();
    }
    
}

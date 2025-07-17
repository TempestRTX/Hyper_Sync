using System.Collections.Generic;
using UnityEngine;

public class GhostPlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpForce = 7f;
    public float smoothing = 10f;
    public float syncDelay = 0.2f;

    private Vector3 targetPosition;
    private bool jumpQueued = false;

    private Queue<(float time, Vector3 delta)> deltaBuffer = new Queue<(float, Vector3)>();

    void Start()
    {
        targetPosition = transform.position;
    }

    void OnEnable()
    {
        EventManager.Instance.Subscribe(GameState.GameEvents.PlayerMoveDelta, OnMoveDelta);
        EventManager.Instance.Subscribe(GameState.GameEvents.PlayerJump, OnJump);
    }

    void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.Unsubscribe(GameState.GameEvents.PlayerMoveDelta, OnMoveDelta);
            EventManager.Instance.Unsubscribe(GameState.GameEvents.PlayerJump, OnJump);
        }
        
    }

    void OnMoveDelta(object data)
    {
        if (data is Vector3 delta)
        {
            Vector3 mirroredDelta = new Vector3(delta.x, 0, 0);
            deltaBuffer.Enqueue((Time.time + syncDelay, mirroredDelta));
        }
    }

    void OnJump(object _)
    {
        jumpQueued = true;
    }

    void Update()
    {
        // Process delayed deltas
        while (deltaBuffer.Count > 0 && Time.time >= deltaBuffer.Peek().time)
        {
            targetPosition += deltaBuffer.Dequeue().delta;
        }

        // Smooth movement
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);

        // Jump
        if (jumpQueued)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpQueued = false;
        }
    }
}
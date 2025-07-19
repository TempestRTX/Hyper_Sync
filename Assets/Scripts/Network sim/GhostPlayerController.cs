using System;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Lane Settings")]
    [SerializeField] private float laneDistance = 2f;   
    [SerializeField] private float laneChangeSpeed = 10f;
    private int currentLane = 1;
    private int totalLanes = 3;
    private Vector3 targetPosition;

    [Header("Simulation")]
    public GameState.EventQueue eventQueue; // Set from GameManager!
    [SerializeField] private bool isAlive = true;
    private Rigidbody rb;
    private Vector3 lastPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = transform.position;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (!isAlive) return;

        // Auto-forward movement (match player)
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Process events from queue (with delay, smoothing)
        var evt = eventQueue.DequeueIfReady(Time.time);
        if (evt.HasValue)
        {
            ProcessEvent(evt.Value);
        }

        // ---- Smooth horizontal movement to new lane ----
        Vector3 laneTarget = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, laneTarget, laneChangeSpeed * Time.deltaTime);
    }

    private void ProcessEvent(GameState.PlayerActionEvent evt)
    {
        switch (evt.type)
        {
            case GameState.PlayerActionType.Jump:
                if (IsGrounded())
                    Jump();
                break;

            case GameState.PlayerActionType.LaneChange:
                // param holds the new lane index
                int newLane = evt.param;
                if (newLane != currentLane)
                {
                    currentLane = newLane;
                    float xPos = (currentLane - 1) * laneDistance;
                    targetPosition = new Vector3(xPos, transform.position.y, transform.position.z);
                    // (You may want to trigger a lane change VFX here)
                }
                break;

            case GameState.PlayerActionType.CollectOrb:
                // Visual: play particle/burst on ghost (optional)
                // Or just ignore to keep ghost "silent"
                break;

            case GameState.PlayerActionType.HitObstacle:
                isAlive = false;
                // Optional: play dissolve shader, shake, stop ghost etc
                break;
        }
    }

    private void Jump()
    {
        if (rb != null && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            // Optional: play jump VFX or sound for ghost
        }
    }

    private bool IsGrounded()
    {
        // Assume a flat ground and simple check. If using ground tags/colliders, you may
        // want to implement the same isGrounded method as your player.
        return Mathf.Abs(rb.linearVelocity.y) < 0.01f;
    }
}

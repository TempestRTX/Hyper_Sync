using System;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] private CameraShake cameraShake;
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
    public GameState.EventQueue eventQueue; 
    [SerializeField] private bool isAlive = true;
    private Rigidbody rb;
    private Vector3 lastPosition;
    
    [SerializeField] private float latency = 0.15f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = transform.position;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (!isAlive) return;

        
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        
        var evt = eventQueue.DequeueIfReady(Time.time - latency);
        if (evt.HasValue)
        {
            ProcessEvent(evt.Value);
        }

        
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
                
                int newLane = evt.param;
                if (newLane != currentLane)
                {
                    currentLane = newLane;
                    float xPos = (currentLane - 1) * laneDistance;
                    targetPosition = new Vector3(xPos, transform.position.y, transform.position.z);
                   
                }
                break;

            case GameState.PlayerActionType.CollectOrb:
                
                break;

            case GameState.PlayerActionType.HitObstacle:
                isAlive = false;
                cameraShake.TriggerShake(1,1);
               
                break;
        }
    }

    private void Jump()
    {
        if (rb != null && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        
        }
    }

    private bool IsGrounded()
    {
       
        return Mathf.Abs(rb.linearVelocity.y) < 0.01f;
    }
}

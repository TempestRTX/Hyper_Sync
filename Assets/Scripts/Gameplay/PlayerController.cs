using System;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float speedIncreaseRate = 0.2f;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private bool isGrounded = true;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        // Auto-forward movement
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Input for jump
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && isGrounded)
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0 && isGrounded)
#endif
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
           
        }

        // Increase speed gradually
        speed += speedIncreaseRate * Time.deltaTime;

        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(GameState.ObjectTags.Ground.ToString()))
        {
            isGrounded = true;
        }
        else if (other.gameObject.CompareTag(GameState.ObjectTags.Collectible.ToString()))
        {
            gameManager.AddScore();
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag(GameState.ObjectTags.Obstacle.ToString()))
        {
            gameManager.GameOver();
        }
    }

    
}
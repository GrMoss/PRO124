using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenBehavior : MonoBehaviour
{
    Animator ani;

    public float detectionRadius = 5f; // Radius within which the critter detects the player
    public float moveSpeed = 2f; // Speed at which the critter moves
    public LayerMask obstacleLayer; // LayerMask to define what counts as an obstacle
    Transform player; // Reference to the player's transform

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(Peck());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
        }
    }

    private void Update()
    {
        if ( player != null)
        {
            // Check the distance between the critter and the player
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer < detectionRadius)
            {
                moveSpeed = 4f * (4f - distanceToPlayer) + 1;
                // Calculate the direction away from the player
                Vector2 directionAwayFromPlayer = (transform.position - player.position).normalized;

                // Check if the path is clear
                if (!IsPathBlocked(directionAwayFromPlayer))
                {
                    // Move the critter in the opposite direction
                    rb.MovePosition(rb.position + directionAwayFromPlayer * moveSpeed * Time.deltaTime);
                    Flip();
                }
                else
                {
                    // Find a new direction to move if the path is blocked
                    Vector2 newDirection = FindAlternativeDirection(directionAwayFromPlayer);
                    rb.MovePosition(rb.position + newDirection * moveSpeed * Time.deltaTime);
                    Flip();
                }

            }
            else
            {
                moveSpeed = 4f;
            }
        }
        
    }

    void Flip()
    {
        if (transform.position.x - player.position.x <= 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    bool IsPathBlocked(Vector2 direction)
    {
        // Cast a ray to check for obstacles
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRadius, obstacleLayer);
        return hit.collider != null;
    }

    Vector2 FindAlternativeDirection(Vector2 originalDirection)
    {
        // Try to find an alternative direction by rotating the original direction
        for (int i = 0; i < 360; i += 45) // Check every 45 degrees
        {
            Vector2 newDirection = Quaternion.Euler(0, 0, i) * originalDirection;
            if (!IsPathBlocked(newDirection))
            {
                return newDirection;
            }
        }
        return -originalDirection; // If no alternative is found, go in the completely opposite direction
    }

    IEnumerator Peck()
    {
        while (true)
        {
            float waitTime = Random.Range(1f, 5f);
            yield return new WaitForSeconds(waitTime);
            if (Random.Range(-1f,1f) >= 0)
            {
                ani.SetTrigger("Peck");
            }
            else
            {
                ani.SetTrigger("Double_Peck");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenBehavior : MonoBehaviour
{
    Animator ani;
    bool isWalking, isRunning;

    public float detectionRadius = 5f; // Radius within which the critter detects the player
    public float moveSpeed = 2f; // Speed at which the critter moves
    public LayerMask obstacleLayer; // LayerMask to define what counts as an obstacle
    Transform player; // Reference to the player's transform

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Vector2 moveDir;
    float moveTimer;
    float walkTime;

    private void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(Peck());

        GetRandomDirection();
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
        // Check the distance between the critter and the player
        float distanceToPlayer;
        if (player != null)
        {
            distanceToPlayer = Vector2.Distance(transform.position, player.position);
        }
        else
        {
            distanceToPlayer = 100f;
        }

        if (distanceToPlayer < detectionRadius)
        {
            isRunning = true;
            ani.SetBool("IsRunning", true);
            moveSpeed = 1f * (4f - distanceToPlayer) * 1.1f + 2f;
            // Calculate the direction away from the player

            Vector2 directionAwayFromPlayer = Vector2.zero;
            float playerX = transform.position.x;

            if (player != null)
            {
                directionAwayFromPlayer = (transform.position - player.position).normalized;
                playerX = player.position.x;
            }

                // Check if the path is clear
            if (!IsPathBlocked(directionAwayFromPlayer))
            {
                // Move the critter in the opposite direction
                rb.MovePosition(rb.position + directionAwayFromPlayer * moveSpeed * Time.deltaTime);
                Flip(playerX);
            }
            else
            {
                // Find a new direction to move if the path is blocked
                Vector2 newDirection = FindAlternativeDirection(directionAwayFromPlayer);
                rb.MovePosition(rb.position + newDirection * moveSpeed * Time.deltaTime);
                Flip(playerX);
            }

        }
        else
        {
            isRunning = false;
            ani.SetBool("IsRunning", false);
        }

        if (player == null || !isRunning)
        {
            moveTimer -= Time.deltaTime;

            if (moveTimer > walkTime)
            {
                isWalking = true;
                ani.SetBool("IsWalking", true);
                moveSpeed = 2f;

                if (!IsPathBlocked(moveDir))
                {
                    // Move the critter in the opposite direction
                    rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);
                    Flip();
                }
                else
                {
                    // Find a new direction to move if the path is blocked
                    Vector2 newDirection = FindAlternativeDirection(moveDir);
                    rb.MovePosition(rb.position + newDirection * moveSpeed * Time.deltaTime);
                    Flip();
                }
            }
            else if (moveTimer > 0f)
            {
                isWalking = false;
                ani.SetBool("IsWalking", false);
            }
            else
            { 
                GetRandomDirection();
            }
            
        }
        else
        {
            if (isWalking)
            {
                moveDir = -moveDir;
            }
        }

    }

    void Flip(float playerX = 0f)
    {
        if (playerX != 0f)
        {
            if (transform.position.x - playerX <= 0)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
        }
        else
        {
            if (moveDir.x <= 0)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
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
            float waitTime = Random.Range(1f, 4f);
            yield return new WaitForSeconds(waitTime);
            if (!isWalking && !isRunning)
            {
                if (Random.Range(-1f, 1f) >= 0)
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

    void GetRandomDirection()
    {
        moveTimer = Random.Range(4f, 10f);
        walkTime = moveTimer - Random.Range(2f, 3f);
        float angle = Random.Range(0f, 360f);
        moveDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class TimmyControls : MonoBehaviour
{
    [Header("Jump")]
    public float jumpForce = 12.5f;
    public float jumpSpeedIncrease = 0.3f;

    [Header("Dash")]
    public float dashDuration = 0.4f;
    public float dashCooldown = 0.8f;

    private float dashTimer = 0f;
    private float cooldownTimer = 0f;

    private bool isDashing = false;
    private bool canDash = true;

    [Header("Spin Animation")]
    public Transform timmySprite;
    public float spinSpeed = 300f;
    public float landingRotationSpeed = 360f;

    [Header("Dash Effects")]
    public ParticleSystem dashEffect;
    private TrailRenderer trailRenderer;

    private Rigidbody2D rb;
    private bool isGrounded = false;

    void Start()
    {
        // Get Timmy's Rigidbody so jumping uses 2D physics
        rb = GetComponent<Rigidbody2D>();

        trailRenderer = GetComponentInChildren<TrailRenderer>();

        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
        }

        if (dashEffect != null)
        {
            dashEffect.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }
    }

    void Update()
    {
        if (GameManager.instance == null ||
            GameManager.instance.isGameOver)
            return;

        if (Keyboard.current == null)
            return;

        // Timmy spins (rotation effect)
        if (timmySprite != null)
        {
            if (!isGrounded)
            {
                timmySprite.Rotate(
                    0f, 0f,
                    -spinSpeed * Time.deltaTime
                );
            }
            else
            {
                timmySprite.localRotation =
                    Quaternion.RotateTowards(
                        timmySprite.localRotation,
                        Quaternion.identity,
                        landingRotationSpeed * Time.deltaTime
                    );
            }
        }

        // Jump while holding space bar
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            float speed = GameManager.instance.gameSpeed;

            float newJumpForce = jumpForce +
                (speed - 5f) * jumpSpeedIncrease;

            // Increase jump force a little as the game gets faster
            rb.AddForce(
                Vector2.up * newJumpForce,
                ForceMode2D.Impulse
            );

            isGrounded = false;

            if (AudioControls.instance != null)
            {
                AudioControls.instance.PlayJump();
            }
        }

        // Dash cooldown control: Count down until Timmy can dash again
        if (!canDash)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                canDash = true;
            }
        }

        // Dash at the start
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame && canDash)
        {
            dashTimer = dashDuration;
            canDash = false;

            AudioControls.instance.PlayDash();
        }

        // Dash while holding right arrow key
        bool shouldDash =
            Keyboard.current.rightArrowKey.isPressed &&
            dashTimer > 0f;

        if (shouldDash)
        {
            dashTimer -= Time.deltaTime;
        }

        // Dash particle effects
        if (shouldDash != isDashing)
        {
            isDashing = shouldDash;

            if (isDashing)
            {
                // Show trail immediately
                if (trailRenderer != null)
                {
                    trailRenderer.Clear();
                    trailRenderer.emitting = true;
                }

                // Restart particles immediately
                if (dashEffect != null)
                {
                    dashEffect.Stop(
                        true,
                        ParticleSystemStopBehavior.StopEmittingAndClear
                    );

                    dashEffect.Play(true);
                }
            }
            else
            {
                dashTimer = 0f;

                // Stop creating trail segments
                if (trailRenderer != null)
                {
                    trailRenderer.emitting = false;
                }

                // Let particles fade naturally
                if (dashEffect != null)
                {
                    dashEffect.Stop(
                        true,
                        ParticleSystemStopBehavior.StopEmitting
                    );
                }

                // Start cooldown
                cooldownTimer = dashCooldown;
            }
        }
    }

    // When Timmy is dashing the GameManager checks this to double the scrolling speed
    public bool IsDashing()
    {
        return isDashing;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    //When Timmy hits an obstacle, end game
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle")) //check only assests with "Obstacle" tag
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver();
            }
        }
    }
}

//AI showed me how to organize my public and private variables better.
//I started using it with my other scripts.
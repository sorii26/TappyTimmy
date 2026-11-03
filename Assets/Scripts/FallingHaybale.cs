
using UnityEngine;

public class FallingHaybale : MonoBehaviour
{
    [Header("Movement")]
    public float airHorizontalMultiplier = 0.35f;
    public float fallSpeed = 5f;

    [Header("Landing")]
    public float groundY = -2.5f;

    private bool hasLanded = false;

    void Update()
    {
        if (GameManager.instance == null ||
            GameManager.instance.isGameOver)
            return;

        float worldSpeed = GameManager.instance.GetWorldSpeed();

        if (!hasLanded)
        {
            // Move slowly left while falling
            transform.position += new Vector3( -worldSpeed * airHorizontalMultiplier, -fallSpeed, 0f) * Time.deltaTime;

            // Stop falling when reaching ground height
            if (transform.position.y <= groundY)
            {
                transform.position = new Vector3(transform.position.x, groundY, transform.position.z);

                hasLanded = true;
            }
        }
        else
        {
            // Move left at normal game speed after landing
            transform.position += Vector3.left * worldSpeed * Time.deltaTime;
        }

        // Remove when off screen
        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }
}

//I wanted the haybales to fly into frame instead of appearing on the ground like the fences.
//I used AI to understand how to transform the haybale position to move at a slight angle
//then locked onto the ground when the two collided.
//Also, simlar to the fence obstacles, I kept the same code for when the obstacle is off the screen.

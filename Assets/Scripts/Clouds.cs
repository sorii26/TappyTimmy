using UnityEngine;

public class Clouds : MonoBehaviour
{
    public float moveSpeed = 1.5f;

    void Update()
    {
        if (GameManager.instance == null || GameManager.instance.isGameOver)
            return;

        // Move each cloud left at a steady speed
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // When the cloud leaves the screen, delete clouds
        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }
}

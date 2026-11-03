using UnityEngine;

public class GroundScroll : MonoBehaviour
{
    public Transform ground1;
    public Transform ground2;

    private float groundWidth;

    void Start()
    {
        // Get the width of ground
        groundWidth = ground1.GetComponent<SpriteRenderer>().bounds.size.x;

        // Place the second ground directly beside the first
        ground2.position = ground1.position + Vector3.right * groundWidth;
    }

    void Update()
    {
        if (GameManager.instance == null || GameManager.instance.isGameOver)
            return;

        // Move at the same speed as the obstacles
        float speed = GameManager.instance.GetWorldSpeed();

        ground1.position += Vector3.left * speed * Time.deltaTime;
        ground2.position += Vector3.left * speed * Time.deltaTime;

        // Move ground1 to the right when it goes off screen
        if (ground1.position.x <= -groundWidth)
        {
            ground1.position += Vector3.right * groundWidth * 2f;
        }

        // Move ground2 to the right when it goes off screen
        if (ground2.position.x <= -groundWidth)
        {
            ground2.position += Vector3.right * groundWidth * 2f;
        }
    }
}

//Used AI to figure out how to create a infinite loop of the ground.

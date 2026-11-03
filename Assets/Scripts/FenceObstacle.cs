using UnityEngine;

public class FenceObstacle : MonoBehaviour
{
    void Update()
    {
        // Check current world speed which allows dashing effects on fence obstacles
        float speed = GameManager.instance.GetWorldSpeed();

        // Move left to keep Timmy moving effect
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);

        //Remove when off screen
        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }
}

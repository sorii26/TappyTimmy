using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    public GameObject fencePrefab;
    public GameObject largeFencePrefab;
    public GameObject haybalePrefab;

    [Header("Spawn Settings")]
    public float spawnRate = 2.5f;
    public float minimumSpawnRate = 1.2f;

    private float timer = 0f;

    void Update()
    {
        if (GameManager.instance == null ||
            GameManager.instance.isGameOver)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnObstacle();
            timer = 0f;

            spawnRate = Mathf.Max(
                minimumSpawnRate,
                spawnRate - 0.03f
            );
        }
    }

    void SpawnObstacle()
    {
        // Random number: 0, 1, or 2
        int randomObstacle = Random.Range(0, 3);

        if (randomObstacle == 0)
        {
            // Regular Fence
            Instantiate(
                fencePrefab,
                new Vector3(10f, -2.5f, 0f),
                Quaternion.identity
            );
        }
        else if (randomObstacle == 1)
        {
            // Large Fence
            Instantiate(
                largeFencePrefab,
                new Vector3(10f, -2.5f, 0f),
                Quaternion.identity
            );
        }
        else
        {
            // Falling Haybale
            Instantiate(
                haybalePrefab,
                new Vector3(10f, 5f, 0f),
                Quaternion.identity
            );
        }
    }
}
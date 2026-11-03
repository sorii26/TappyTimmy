using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    //Cloud Prefabs
    public GameObject cloud1Prefab;
    public GameObject cloud2Prefab;

    //Spawn Times
    public float minSpawnTime = 2f;
    public float maxSpawnTime = 4f;

    //Spawn Heights
    public float spawnX = 12f;
    public float minY = 2f;      
    public float maxY = 4.5f;   

    //Next spawn occurance
    private float spawnTimer;
    private float nextSpawnTime;

    void Start()
    {
        SetNextSpawnTime();
    }

    void Update()
    {
        if (GameManager.instance == null || GameManager.instance.isGameOver)
            return;

        // Add the time passed each frame to the spawn timer
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= nextSpawnTime)
        {
            SpawnCloud();
            spawnTimer = 0f;
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        // Clouds spawn at random times
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void SpawnCloud()
    {
        GameObject cloudToSpawn;

        // Randomly chooses one of the two cloud prefabs
        if (Random.Range(0, 2) == 0)
        {
            cloudToSpawn = cloud1Prefab;
        }
        else
        {
            cloudToSpawn = cloud2Prefab;
        }

        
        // Clouds spawn at different heights
        float randomY = Random.Range(minY, maxY);

        GameObject newCloud = Instantiate(
            cloudToSpawn,
            new Vector3(spawnX, randomY, 0f),
            Quaternion.identity
        );

        // Clouds spawn at different sizes
        float randomScale = Random.Range(0.8f, 1.3f);
        newCloud.transform.localScale = new Vector3(randomScale, randomScale, 1f);
    }
}
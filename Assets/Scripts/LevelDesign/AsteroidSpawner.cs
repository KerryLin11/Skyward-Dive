using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject[] rockPrefabs;
    public float spawnInterval = 2f;
    private const string WALL_TAG = "Wall";
    private const int WALL_LAYER = 3;

    public float spawnRangeX = 5f, spawnRangeZ = 5f;

    private void Start()
    {
        if (rockPrefabs.Length > 0)
        {
            InvokeRepeating(nameof(SpawnRock), 0f, spawnInterval);
        }
        else
        {
            Debug.LogError("No rocks assignwed");
        }
    }

    void SpawnRock()
    {
        GameObject randomRockPrefab = rockPrefabs[Random.Range(0, rockPrefabs.Length)];
        Vector3 randomOffset = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0f, Random.Range(-spawnRangeZ, spawnRangeZ));
        Vector3 randomSpawnPosition = transform.position + randomOffset;
        Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        GameObject newRock = Instantiate(randomRockPrefab, randomSpawnPosition, randomRotation);
        newRock.tag = WALL_TAG;
        newRock.layer = WALL_LAYER;

        newRock.transform.SetParent(transform);


        //! Destroy rock after 10s
        Destroy(newRock, 10f);
    }

}

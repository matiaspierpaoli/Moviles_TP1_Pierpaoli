using System.Collections.Generic;
using UnityEngine;

public interface ISpawnable
{
    void SetSpawnPoint(Transform point, SpawnManager manager);
}

[System.Serializable]
public class SpawnSettings
{
    public string objectTag;
    public bool isMultiSpawneable;
    public float easySpawnInterval;
    public float mediumSpawnInterval;
    public float hardSpawnInterval;
    public int easyMinObjectsToSpawn;
    public int easyMaxObjectsToSpawn;
    public int mediumMinObjectsToSpawn;
    public int mediumMaxObjectsToSpawn;
    public int hardMinObjectsToSpawn;
    public int hardMaxObjectsToSpawn;
}

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public SpawnSettings spawnSettings;
    public GameSettings gameSettings; 

    public CheckPointsHolder checkPointsHolder;
    public int maxCheckpointsFromPlayer = 4;
    private int lastActiveCheckpoint = -1;

    private List<Transform> occupiedSpawnPoints = new List<Transform>();
    private float spawnTimer;

    private Dictionary<Difficulty, float> spawnIntervalByDifficulty;
    private Dictionary<Difficulty, (int min, int max)> objectsToSpawnByDifficulty;

    private void Start()
    {
        InitializeDictionaries();
        SetSpawnSettingsByDifficulty();

        if (!spawnSettings.isMultiSpawneable)
            SpawnInitialObjects();
    }

    private void Update()
    {
        if (spawnSettings.isMultiSpawneable)
            CheckObjectState();
    }

    private void InitializeDictionaries()
    {
        spawnIntervalByDifficulty = new Dictionary<Difficulty, float>
        {
            { Difficulty.Easy, spawnSettings.easySpawnInterval },
            { Difficulty.Medium, spawnSettings.mediumSpawnInterval },
            { Difficulty.Hard, spawnSettings.hardSpawnInterval }
        };

        objectsToSpawnByDifficulty = new Dictionary<Difficulty, (int min, int max)>
        {
            { Difficulty.Easy, (spawnSettings.easyMinObjectsToSpawn, spawnSettings.easyMaxObjectsToSpawn) },
            { Difficulty.Medium, (spawnSettings.mediumMinObjectsToSpawn, spawnSettings.mediumMaxObjectsToSpawn) },
            { Difficulty.Hard, (spawnSettings.hardMinObjectsToSpawn, spawnSettings.hardMaxObjectsToSpawn) }
        };
    }

    private void SetSpawnSettingsByDifficulty()
    {
        Difficulty difficulty = gameSettings.currentDifficulty;
        spawnTimer = spawnIntervalByDifficulty[difficulty];
    }

    private void SpawnInitialObjects()
    {
        Difficulty difficulty = gameSettings.currentDifficulty;
        var (minObjects, maxObjects) = objectsToSpawnByDifficulty[difficulty];
        int numberOfObjectsToSpawn = Random.Range(minObjects, maxObjects);

        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        for (int i = 0; i < numberOfObjectsToSpawn; i++)
        {
            if (availableSpawnPoints.Count == 0)
            {
                Debug.LogWarning("No more available spawn points.");
                break;
            }

            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[randomIndex];

            availableSpawnPoints.RemoveAt(randomIndex);

            SpawnObjectAtPoint(spawnPoint);
        }
    }

    private void SpawnObjectAtPoint(Transform spawnPoint)
    {
        GameObject obj = ObjectPool.Instance.GetObjectFromPool(spawnSettings.objectTag);
        if (obj != null)
        {
            obj.transform.position = spawnPoint.position;
            obj.transform.rotation = spawnPoint.rotation;
            obj.SetActive(true);

            occupiedSpawnPoints.Add(spawnPoint);

            ISpawnable spawnable = obj.GetComponent<ISpawnable>();
            if (spawnable != null)
            {
                spawnable.SetSpawnPoint(spawnPoint, this);
            }
        }
    }

    private void CheckObjectState()
    {
        if (GameManager.Instancia.GetCurrentState() is PlayingState)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0f)
            {
                UpdateLastCheckPoint();
                SpawnObject();
                spawnTimer = spawnIntervalByDifficulty[gameSettings.currentDifficulty];
            }
        }
    }

    private void UpdateLastCheckPoint()
    {
        for (int i = 0; i < checkPointsHolder.checkPoints.Count; i++)
        {
            if (checkPointsHolder.checkPoints[i].BothPlayersPassed())
            {
                lastActiveCheckpoint = i;
            }
        }
    }

    private void SpawnObject()
    {
        List<Transform> validPoints = new List<Transform>();

        int finalCheckPoint = Mathf.Min(lastActiveCheckpoint + maxCheckpointsFromPlayer, spawnPoints.Length - 1);

        for (int i = lastActiveCheckpoint + 1; i <= finalCheckPoint; i++)
        {
            if (!occupiedSpawnPoints.Contains(spawnPoints[i]))
            {
                validPoints.Add(spawnPoints[i]);
            }
        }

        if (validPoints.Count > 0)
        {
            Transform spawnPoint = validPoints[Random.Range(0, validPoints.Count)];
            SpawnObjectAtPoint(spawnPoint);
        }
        else
        {
            Debug.LogWarning("No more spawn points available.");
        }
    }

    public void ReleaseSpawnPoint(Transform spawnPoint)
    {
        if (occupiedSpawnPoints.Contains(spawnPoint))
        {
            occupiedSpawnPoints.Remove(spawnPoint);
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public float spawnInterval = 5f; 
    private float spawnTimer;

    public CheckPointsHolder checkPointsHolder; 
    public int maxCheckpointsFromPlayer = 4;
    private int lastActiveCheckpoint = -1;

    public string objectTag;

    private List<Transform> occupiedSpawnPoints = new List<Transform>();

    private void Start()
    {
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        CheckObjectState();
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
                spawnTimer = spawnInterval;
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

            GameObject obj = ObjectPool.Instance.GetObjectFromPool(objectTag);
            if (obj != null)
            {
                obj.transform.position = spawnPoint.position;
                obj.SetActive(true);

                occupiedSpawnPoints.Add(spawnPoint);

                obj.GetComponent<Bolsa>().SetSpawnPoint(spawnPoint, this);
            }
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

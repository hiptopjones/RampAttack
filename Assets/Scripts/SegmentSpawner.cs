using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentSpawner : MonoBehaviour
{
    [SerializeField] Vector3 startSpawnPosition;

    [SerializeField] GameObject roadPrefab;
    [SerializeField] GameObject archPrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject[] rampPrefabs;

    [SerializeField] float spawnInterval = 2.5f;
    [SerializeField] float initialSpawnCount = 2;
    [SerializeField] float segmentLength = 75;
    [SerializeField] float archHeight = 3;
    [SerializeField] float[] rampOffsets;
    [SerializeField] float[] coinOffsets;

    private int numSegmentsSpawned = 0;
    private bool isSpawning;

    Queue<GameObject> spawnedObjects = new Queue<GameObject>();

    private void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        if (isSpawning)
        {
            throw new System.Exception("Spawning already started");
        }

        isSpawning = true;
        StartCoroutine(SpawnSegments());
    }

    public void StopSpawning()
    {
        if (false == isSpawning)
        {
            throw new System.Exception("Spawning not started");
        }

        isSpawning = false;
    }

    IEnumerator SpawnSegments()
    {
        SpawnStartSegment();

        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnSegment();
        }

        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnSegment();
        }
    }

    void SpawnStartSegment()
    {
        SpawnObject(roadPrefab, startSpawnPosition);
        numSegmentsSpawned++;
    }

    void SpawnSegment()
    {
        Vector3 segmentPosition = startSpawnPosition + Vector3.forward * numSegmentsSpawned * segmentLength;
        SpawnObject(roadPrefab, segmentPosition);

        int numArches = Random.Range(1, 4);
        for (int i = 0; i < numArches; i++)
        {
            Vector3 archPosition = segmentPosition + Vector3.up * i * archHeight;
            SpawnObject(archPrefab, archPosition);
        }

        //Vector3 rampPosition = segmentPosition - Vector3.forward * rampOffsets[numArches - 1];
        //SpawnObject(rampPrefabs[numArches - 1], rampPosition);
 
        for (int i = 0; i < 5; i++)
        {
            float coinOffset = coinOffsets[numArches - 1] + i * 4;

            Vector3 coinPosition = segmentPosition + Vector3.forward * coinOffset + Vector3.up;
            SpawnObject(coinPrefab, coinPosition);
        }

        numSegmentsSpawned++;
    }

    private void SpawnObject(GameObject prefab, Vector3 position)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        spawnedObjects.Enqueue(obj);
    }
}

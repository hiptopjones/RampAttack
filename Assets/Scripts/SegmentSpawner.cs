using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentSpawner : MonoBehaviour
{
    [SerializeField] Vector3 startSpawnPosition;

    [SerializeField] GameObject roadPrefab;
    [SerializeField] GameObject archPrefab;
    [SerializeField] GameObject coinPrefab;

    [SerializeField] float spawnAheadDistance = 200;
    [SerializeField] float segmentLength = 75;
    [SerializeField] float archHeight = 3;
    [SerializeField] float archSpacing = 3;
    [SerializeField] int maxArchesUp = 3;
    [SerializeField] int maxArchesDeep = 3;
    [SerializeField] float[] coinOffsets;
    [SerializeField] float numCoinsPerSegment = 5;
    [SerializeField] float coinSpacing = 4;

    private int numSegmentsSpawned = 0;
    private bool isSpawning;

    ResourceManager resourceManager;
    VehicleController vehicleController;

    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        vehicleController = FindObjectOfType<VehicleController>();

        StartSpawning();
    }

    void Update()
    {
        if (isSpawning)
        {
            Vector3 nextSegmentPosition = startSpawnPosition + Vector3.forward * numSegmentsSpawned * segmentLength;
            Vector3 playerCurrentPosition = vehicleController.transform.position;

            if (nextSegmentPosition.z - spawnAheadDistance < playerCurrentPosition.z)
            {
                SpawnSegment(nextSegmentPosition);
            }
        }
    }

    public void StartSpawning()
    {
        if (isSpawning)
        {
            throw new System.Exception("Spawning already started");
        }

        isSpawning = true;

        SpawnStartSegment();
    }

    public void StopSpawning()
    {
        if (false == isSpawning)
        {
            throw new System.Exception("Spawning not started");
        }

        isSpawning = false;
    }

    void SpawnStartSegment()
    {
        GameObject road = resourceManager.GetOrCreateRoad();
        road.transform.position = startSpawnPosition;

        numSegmentsSpawned++;
    }

    void SpawnSegment(Vector3 segmentPosition)
    {
        GameObject road = resourceManager.GetOrCreateRoad();
        road.transform.position = segmentPosition;

        int numArchesUp = Random.Range(1, maxArchesUp + 1);
        int numArchesDeep = Random.Range(1, maxArchesDeep + 1);
        for (int z = 0; z < numArchesDeep; z++)
        {
            for (int y = 0; y < numArchesUp; y++)
            {
                GameObject arch = resourceManager.GetOrCreateArch();
                arch.transform.position = segmentPosition + (Vector3.up * y * archHeight) + (Vector3.forward * z * archSpacing);
            }
        }

        for (int i = 0; i < numCoinsPerSegment; i++)
        {
            float coinOffset = coinOffsets[numArchesUp - 1] + ((numArchesDeep - 1) * archSpacing) + (i * coinSpacing);

            GameObject coin = resourceManager.GetOrCreateCoin();
            coin.transform.position = segmentPosition + Vector3.forward * coinOffset;
        }

        numSegmentsSpawned++;
    }
}

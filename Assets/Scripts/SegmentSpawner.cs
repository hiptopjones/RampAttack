﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentSpawner : MonoBehaviour
{
    [SerializeField] Vector3 startSpawnPosition;

    [SerializeField] GameObject roadPrefab;
    [SerializeField] GameObject archPrefab;
    [SerializeField] GameObject coinPrefab;

    [SerializeField] float spawnInterval = 2.5f;
    [SerializeField] float initialSpawnCount = 2;
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

    private void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();

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
        GameObject road = resourceManager.GetOrCreateRoad();
        road.transform.position = startSpawnPosition;

        numSegmentsSpawned++;
    }

    void SpawnSegment()
    {
        Vector3 segmentPosition = startSpawnPosition + Vector3.forward * numSegmentsSpawned * segmentLength;

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

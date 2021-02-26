﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ObjectType
{
    Road,
    Arch1,
    Arch2,
    Arch3,
    Coin,
    CoinEffect,
    SmallRamp,
    MediumRamp,
    LargeRamp
};

public class ResourceManager : MonoBehaviour
{
    [SerializeField] float reclaimInterval = 10;
    [SerializeField] float reclaimDistance = 80;

    [SerializeField] GameObject roadPrefab;
    [SerializeField] GameObject arch1Prefab;
    [SerializeField] GameObject arch2Prefab;
    [SerializeField] GameObject arch3Prefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject coinEffectPrefab;
    [SerializeField] GameObject smallRampPrefab;
    [SerializeField] GameObject mediumRampPrefab;
    [SerializeField] GameObject largeRampPrefab;

    Dictionary<ObjectType, HashSet<GameObject>> activeObjects = new Dictionary<ObjectType, HashSet<GameObject>>();
    Dictionary<ObjectType, Stack<GameObject>> freeObjects = new Dictionary<ObjectType, Stack<GameObject>>();

    VehiclePhysicsController vehiclePhysicsController;

    float startTime;

    void Start()
    {
        vehiclePhysicsController = FindObjectOfType<VehiclePhysicsController>();

        activeObjects[ObjectType.Road] = new HashSet<GameObject>();
        activeObjects[ObjectType.Arch1] = new HashSet<GameObject>();
        activeObjects[ObjectType.Arch2] = new HashSet<GameObject>();
        activeObjects[ObjectType.Arch3] = new HashSet<GameObject>();
        activeObjects[ObjectType.Coin] = new HashSet<GameObject>();
        activeObjects[ObjectType.CoinEffect] = new HashSet<GameObject>();
        activeObjects[ObjectType.SmallRamp] = new HashSet<GameObject>();
        activeObjects[ObjectType.MediumRamp] = new HashSet<GameObject>();
        activeObjects[ObjectType.LargeRamp] = new HashSet<GameObject>();

        freeObjects[ObjectType.Road] = new Stack<GameObject>();
        freeObjects[ObjectType.Arch1] = new Stack<GameObject>();
        freeObjects[ObjectType.Arch2] = new Stack<GameObject>();
        freeObjects[ObjectType.Arch3] = new Stack<GameObject>();
        freeObjects[ObjectType.Coin] = new Stack<GameObject>();
        freeObjects[ObjectType.CoinEffect] = new Stack<GameObject>();
        freeObjects[ObjectType.SmallRamp] = new Stack<GameObject>();
        freeObjects[ObjectType.MediumRamp] = new Stack<GameObject>();
        freeObjects[ObjectType.LargeRamp] = new Stack<GameObject>();

        startTime = Time.time;
    }

    void Update()
    {
        int elapsedSeconds = (int)(Time.time - startTime);
        if (elapsedSeconds % reclaimInterval == 0)
        {
            ReclaimObjects();
        }
    }

    private void ReclaimObjects()
    {
        // TODO: This should not be here.
        // The resource manager should be notified that it can reclaim objects meeting some condition
        float currentVehicleZ = vehiclePhysicsController.transform.position.z;
        float candidateObjectZ = currentVehicleZ - reclaimDistance;

        foreach (ObjectType objectType in Enum.GetValues(typeof(ObjectType)))
        {
            ReclaimObjects(objectType, candidateObjectZ);
        }
    }

    private void ReclaimObjects(ObjectType objectType, float candidateObjectZ)
    {
        // TODO: Put this in a more global space to avoid constant reallocation
        Queue<GameObject> candidateObjects = new Queue<GameObject>();

        // Put candidates into queue to avoid invalidating the iterator
        foreach (GameObject gameObject in activeObjects[objectType])
        {
            if (gameObject.transform.position.z < candidateObjectZ)
            {
                candidateObjects.Enqueue(gameObject);
            }
        }

        while (candidateObjects.Any())
        {
            DestroyObject(objectType, candidateObjects.Dequeue());
        }
    }

    public void DestroyRoad(GameObject road)
    {
        DestroyObject(ObjectType.Road, road);
    }

    public void DestroyArch1(GameObject arch)
    {
        DestroyObject(ObjectType.Arch1, arch);
    }

    public void DestroyArch2(GameObject arch)
    {
        DestroyObject(ObjectType.Arch2, arch);
    }

    public void DestroyArch3(GameObject arch)
    {
        DestroyObject(ObjectType.Arch3, arch);
    }

    public void DestroyCoin(GameObject coin)
    {
        DestroyObject(ObjectType.Coin, coin);
    }

    public void DestroyCoinEffect(GameObject effect)
    {
        DestroyObject(ObjectType.CoinEffect, effect);
    }

    public void DestroySmallRamp(GameObject ramp)
    {
        DestroyObject(ObjectType.SmallRamp, ramp);
    }

    public void DestroyMediumRamp(GameObject ramp)
    {
        DestroyObject(ObjectType.MediumRamp, ramp);
    }

    public void DestroyLargeRamp(GameObject ramp)
    {
        DestroyObject(ObjectType.LargeRamp, ramp);
    }

    private void DestroyObject(ObjectType objectType, GameObject gameObject)
    {
        gameObject.SetActive(false);
        activeObjects[objectType].Remove(gameObject);
        freeObjects[objectType].Push(gameObject);
    }

    public GameObject GetOrCreateRoad()
    {
        return GetOrCreateObject(ObjectType.Road);
    }

    public GameObject GetOrCreateArch1()
    {
        return GetOrCreateObject(ObjectType.Arch1);
    }

    public GameObject GetOrCreateArch2()
    {
        return GetOrCreateObject(ObjectType.Arch2);
    }

    public GameObject GetOrCreateArch3()
    {
        return GetOrCreateObject(ObjectType.Arch3);
    }

    public GameObject GetOrCreateCoin()
    {
        return GetOrCreateObject(ObjectType.Coin);
    }

    public GameObject GetOrCreateCoinEffect()
    {
        return GetOrCreateObject(ObjectType.CoinEffect);
    }

    public GameObject GetOrCreateSmallRamp()
    {
        return GetOrCreateObject(ObjectType.SmallRamp);
    }

    public GameObject GetOrCreateMediumRamp()
    {
        return GetOrCreateObject(ObjectType.MediumRamp);
    }

    public GameObject GetOrCreateLargeRamp()
    {
        return GetOrCreateObject(ObjectType.LargeRamp);
    }

    public GameObject GetOrCreateObject(ObjectType objectType)
    {
        GameObject gameObject;

        if (freeObjects[objectType].Any())
        {
            gameObject = freeObjects[objectType].Pop();
            gameObject.SetActive(true);
        }
        else
        {
            GameObject prefab = GetPrefab(objectType);
            gameObject = Instantiate(prefab, transform.position, Quaternion.identity);
        }

        activeObjects[objectType].Add(gameObject);
        return gameObject;
    }

    private GameObject GetPrefab(ObjectType objectType)
    {
        switch (objectType)
        {
            case ObjectType.Road:
                return roadPrefab;

            case ObjectType.Arch1:
                return arch1Prefab;

            case ObjectType.Arch2:
                return arch2Prefab;

            case ObjectType.Arch3:
                return arch3Prefab;

            case ObjectType.Coin:
                return coinPrefab;

            case ObjectType.CoinEffect:
                return coinEffectPrefab;

            case ObjectType.SmallRamp:
                return smallRampPrefab;

            case ObjectType.MediumRamp:
                return mediumRampPrefab;

            case ObjectType.LargeRamp:
                return largeRampPrefab;

            default:
                throw new System.Exception("Unknown object type: " + objectType);
        }
    }
}

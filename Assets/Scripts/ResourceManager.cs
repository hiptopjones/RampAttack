using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ObjectType
{
    StraightRoad,
    TransitionRoad,
    Arch1,
    Arch2,
    Arch3,
    Coin,
    CoinEffect,
    SmallRamp,
    MediumRamp,
    LargeRamp,
    Building,
    Checkpoint
};

public class ResourceManager : MonoBehaviour
{
    [SerializeField] float reclaimInterval = 10;
    [SerializeField] float reclaimDistance = 80;

    [SerializeField] GameObject straightRoadPrefab;
    [SerializeField] GameObject transitionRoadPrefab;
    [SerializeField] GameObject arch1Prefab;
    [SerializeField] GameObject arch2Prefab;
    [SerializeField] GameObject arch3Prefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject coinEffectPrefab;
    [SerializeField] GameObject smallRampPrefab;
    [SerializeField] GameObject mediumRampPrefab;
    [SerializeField] GameObject largeRampPrefab;
    [SerializeField] GameObject buildingPrefab;
    [SerializeField] GameObject checkpointPrefab;

    private Dictionary<ObjectType, HashSet<GameObject>> activeObjects = new Dictionary<ObjectType, HashSet<GameObject>>();
    private Dictionary<ObjectType, Stack<GameObject>> freeObjects = new Dictionary<ObjectType, Stack<GameObject>>();

    private VehiclePhysicsController vehiclePhysicsController;

    private float startTime;

    void Start()
    {
        vehiclePhysicsController = FindObjectOfType<VehiclePhysicsController>();
        if (vehiclePhysicsController == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(VehiclePhysicsController)}");
        }

        activeObjects[ObjectType.StraightRoad] = new HashSet<GameObject>();
        activeObjects[ObjectType.TransitionRoad] = new HashSet<GameObject>();
        activeObjects[ObjectType.Arch1] = new HashSet<GameObject>();
        activeObjects[ObjectType.Arch2] = new HashSet<GameObject>();
        activeObjects[ObjectType.Arch3] = new HashSet<GameObject>();
        activeObjects[ObjectType.Coin] = new HashSet<GameObject>();
        activeObjects[ObjectType.CoinEffect] = new HashSet<GameObject>();
        activeObjects[ObjectType.SmallRamp] = new HashSet<GameObject>();
        activeObjects[ObjectType.MediumRamp] = new HashSet<GameObject>();
        activeObjects[ObjectType.LargeRamp] = new HashSet<GameObject>();
        activeObjects[ObjectType.Building] = new HashSet<GameObject>();
        activeObjects[ObjectType.Checkpoint] = new HashSet<GameObject>();

        freeObjects[ObjectType.StraightRoad] = new Stack<GameObject>();
        freeObjects[ObjectType.TransitionRoad] = new Stack<GameObject>();
        freeObjects[ObjectType.Arch1] = new Stack<GameObject>();
        freeObjects[ObjectType.Arch2] = new Stack<GameObject>();
        freeObjects[ObjectType.Arch3] = new Stack<GameObject>();
        freeObjects[ObjectType.Coin] = new Stack<GameObject>();
        freeObjects[ObjectType.CoinEffect] = new Stack<GameObject>();
        freeObjects[ObjectType.SmallRamp] = new Stack<GameObject>();
        freeObjects[ObjectType.MediumRamp] = new Stack<GameObject>();
        freeObjects[ObjectType.LargeRamp] = new Stack<GameObject>();
        freeObjects[ObjectType.Building] = new Stack<GameObject>();
        freeObjects[ObjectType.Checkpoint] = new Stack<GameObject>();

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
        if (vehiclePhysicsController == null)
        {
            return;
        }

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

    public void DestroyStraightRoad(GameObject road)
    {
        DestroyObject(ObjectType.StraightRoad, road);
    }

    public void DestroyTransitionRoad(GameObject road)
    {
        DestroyObject(ObjectType.TransitionRoad, road);
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

    public void DestroyBuilding(GameObject building)
    {
        DestroyObject(ObjectType.Building, building);
    }

    public void DestroyCheckpoint(GameObject checkpoint)
    {
        DestroyObject(ObjectType.Checkpoint, checkpoint);
    }

    private void DestroyObject(ObjectType objectType, GameObject gameObject)
    {
        gameObject.SetActive(false);
        activeObjects[objectType].Remove(gameObject);
        freeObjects[objectType].Push(gameObject);
    }

    public GameObject GetOrCreateStraightRoad()
    {
        return GetOrCreateObject(ObjectType.StraightRoad);
    }

    public GameObject GetOrCreateTransitionRoad()
    {
        return GetOrCreateObject(ObjectType.TransitionRoad);
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

    public GameObject GetOrCreateBuilding()
    {
        return GetOrCreateObject(ObjectType.Building);
    }

    public GameObject GetOrCreateCheckpoint()
    {
        return GetOrCreateObject(ObjectType.Checkpoint);
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
            case ObjectType.TransitionRoad:
                return transitionRoadPrefab;

            case ObjectType.StraightRoad:
                return straightRoadPrefab;

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

            case ObjectType.Building:
                return buildingPrefab;

            case ObjectType.Checkpoint:
                return checkpointPrefab;

            default:
                throw new System.Exception("Unknown object type: " + objectType);
        }
    }
}

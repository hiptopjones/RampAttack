using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ObjectType
{
    Road,
    Arch,
    Coin,
    SmallRamp,
    MediumRamp,
    LargeRamp
};

public class ResourceManager : MonoBehaviour
{
    [SerializeField] float reclaimInterval = 10;
    [SerializeField] float reclaimDistance = 80;

    [SerializeField] GameObject roadPrefab;
    [SerializeField] GameObject archPrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject smallRampPrefab;
    [SerializeField] GameObject mediumRampPrefab;
    [SerializeField] GameObject largeRampPrefab;

    Dictionary<ObjectType, HashSet<GameObject>> activeObjects = new Dictionary<ObjectType, HashSet<GameObject>>();
    Dictionary<ObjectType, Stack<GameObject>> freeObjects = new Dictionary<ObjectType, Stack<GameObject>>();

    VehicleController vehicleController;

    float startTime;

    void Start()
    {
        vehicleController = FindObjectOfType<VehicleController>();

        activeObjects[ObjectType.Road] = new HashSet<GameObject>();
        activeObjects[ObjectType.Arch] = new HashSet<GameObject>();
        activeObjects[ObjectType.Coin] = new HashSet<GameObject>();
        activeObjects[ObjectType.SmallRamp] = new HashSet<GameObject>();
        activeObjects[ObjectType.MediumRamp] = new HashSet<GameObject>();
        activeObjects[ObjectType.LargeRamp] = new HashSet<GameObject>();

        freeObjects[ObjectType.Road] = new Stack<GameObject>();
        freeObjects[ObjectType.Arch] = new Stack<GameObject>();
        freeObjects[ObjectType.Coin] = new Stack<GameObject>();
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
        float currentVehicleZ = vehicleController.transform.position.z;
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

    public void DestroyArch(GameObject arch)
    {
        DestroyObject(ObjectType.Arch, arch);
    }

    public void DestroyCoin(GameObject coin)
    {
        DestroyObject(ObjectType.Coin, coin);
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

    public GameObject GetOrCreateArch()
    {
        return GetOrCreateObject(ObjectType.Arch);
    }

    public GameObject GetOrCreateCoin()
    {
        return GetOrCreateObject(ObjectType.Coin);
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

            case ObjectType.Arch:
                return archPrefab;

            case ObjectType.Coin:
                return coinPrefab;

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

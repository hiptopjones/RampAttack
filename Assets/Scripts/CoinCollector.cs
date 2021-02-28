using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] Vector3 uiOffset;

    private VehiclePhysicsController vehiclePhysicsController;

    // Start is called before the first frame update
    void Start()
    {
        vehiclePhysicsController = FindObjectOfType<VehiclePhysicsController>();
        if (vehiclePhysicsController == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(VehiclePhysicsController)}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (vehiclePhysicsController == null)
        {
            return;
        }

        transform.position = vehiclePhysicsController.transform.position + uiOffset;
    }
}

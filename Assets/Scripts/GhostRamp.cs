using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRamp : MonoBehaviour
{
    [SerializeField] float ghostOffset;

    private ResourceManager resourceManager;
    private VehiclePhysicsController vehiclePhysicsController;

    private GameObject ramp;
    private KeyCode keyCode;

    private bool isRampPlaced = false;

    // Start is called before the first frame update
    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        if (resourceManager == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(ResourceManager)}");
        }

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

        // Prevent any ramps before the player starts moving
        if (false == vehiclePhysicsController.IsRunning())
        {
            return;
        }

        if (vehiclePhysicsController.IsGrounded())
        {
            if (ramp == null)
            {
                if (Input.GetKey(KeyCode.Alpha1))
                {
                    ramp = resourceManager.GetOrCreateSmallRamp();
                    keyCode = KeyCode.Alpha1;
                }
                else if (Input.GetKey(KeyCode.Alpha2))
                {
                    ramp = resourceManager.GetOrCreateMediumRamp();
                    keyCode = KeyCode.Alpha2;
                }
                else if (Input.GetKey(KeyCode.Alpha3))
                {
                    ramp = resourceManager.GetOrCreateLargeRamp();
                    keyCode = KeyCode.Alpha3;
                }
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.Alpha1))
                {
                    isRampPlaced = true;
                }
                else if (Input.GetKeyUp(KeyCode.Alpha2))
                {

                    isRampPlaced = true;
                }
                else if (Input.GetKeyUp(KeyCode.Alpha3))
                {
                    isRampPlaced = true;
                }
            }
        }
        else if (vehiclePhysicsController.IsAirborne())
        {
            isRampPlaced = false;
            ramp = null;
        }

        if (ramp != null && false == isRampPlaced)
        {
            // Ensure the ghost always shows in the right place
            ramp.transform.position = vehiclePhysicsController.transform.position + Vector3.forward * ghostOffset;
        }
    }
}
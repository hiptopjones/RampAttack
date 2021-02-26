using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRamp : MonoBehaviour
{
    [SerializeField] float ghostOffset;

    private ResourceManager resourceManager;
    private MeshRenderer meshRenderer;
    private VehiclePhysicsController vehiclePhysicsController;

    private bool isRampPlaced = false;

    // Start is called before the first frame update
    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        if (resourceManager == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(ResourceManager)}");
        }

        meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (meshRenderer == null)
        {
            throw new System.Exception($"Unable to get component in children of type {nameof(MeshRenderer)}");
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

        // Ensure the ghost always shows in the right place
        transform.position = vehiclePhysicsController.transform.position + Vector3.forward * ghostOffset;

        // Prevent any ramps before the player starts moving
        if (false == vehiclePhysicsController.IsRunning())
        {
            return;
        }

        if (isRampPlaced)
        {
            if (vehiclePhysicsController.IsAirborne())
            {
                isRampPlaced = false;
            }
        }
        else
        {
            if (vehiclePhysicsController.IsGrounded())
            {
                GameObject ramp = null;

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    ramp = resourceManager.GetOrCreateSmallRamp();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    ramp = resourceManager.GetOrCreateMediumRamp();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    ramp = resourceManager.GetOrCreateLargeRamp();
                }

                if (ramp != null)
                {
                    ramp.transform.position = transform.position;
                    isRampPlaced = true;

                    // Disable right away to see if this prevents looking like the marker continues up the ramp
                    meshRenderer.enabled = false;
                }
                else
                {
                    meshRenderer.enabled = true;
                }
            }
            else
            {
                meshRenderer.enabled = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampManager : MonoBehaviour
{
    [SerializeField] float ghostOffset;
    [SerializeField] Material ghostMaterial;
    [SerializeField] Material rampMaterial;

    private ResourceManager resourceManager;
    private VehiclePhysicsController vehiclePhysicsController;

    private GameObject rampGhost;
    private KeyCode rampKeyCode;

    private bool isRampSelected = false;
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

        // Can only select and place ramps while on the ground
        if (vehiclePhysicsController.IsGrounded())
        {
            // User selects a ramp by pressing down a key
            // User places a ramp by releasing the same key

            if (false == isRampSelected)
            {
                if (Input.GetKey(KeyCode.Alpha1))
                {
                    isRampSelected = true;
                    rampGhost = resourceManager.GetOrCreateSmallRamp();
                    rampKeyCode = KeyCode.Alpha1;
                }
                else if (Input.GetKey(KeyCode.Alpha2))
                {
                    isRampSelected = true;
                    rampGhost = resourceManager.GetOrCreateMediumRamp();
                    rampKeyCode = KeyCode.Alpha2;
                }
                else if (Input.GetKey(KeyCode.Alpha3))
                {
                    isRampSelected = true;
                    rampGhost = resourceManager.GetOrCreateLargeRamp();
                    rampKeyCode = KeyCode.Alpha3;
                }

                // If ramp was selected, give it a ghost-like appearance
                if (isRampSelected)
                {
                    Renderer[] renderers = rampGhost.GetComponentsInChildren<Renderer>();
                    foreach (Renderer renderer in renderers)
                    {
                        renderer.material = ghostMaterial;
                    }
                }
            }
            else if (false == isRampPlaced)
            {
                if (Input.GetKeyUp(rampKeyCode))
                {
                    isRampPlaced = true;
                }

                // If ramp was placed, remove the ghost appearance
                if (isRampPlaced)
                {
                    Renderer[] renderers = rampGhost.GetComponentsInChildren<Renderer>();
                    foreach (Renderer renderer in renderers)
                    {
                        renderer.material = rampMaterial;
                    }
                }
            }

            // Make the ghost precede the player's position
            if (isRampSelected && false == isRampPlaced)
            {
                rampGhost.transform.position = vehiclePhysicsController.transform.position + Vector3.forward * ghostOffset;
            }
        }
        else if (vehiclePhysicsController.IsAirborne())
        {
            // Reset once we hit the air
            isRampSelected = false;
            isRampPlaced = false;
            rampGhost = null;
        }
    }
}
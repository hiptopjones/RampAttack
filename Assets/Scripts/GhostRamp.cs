using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRamp : MonoBehaviour
{
    [SerializeField] float ghostOffset;

    private MeshRenderer meshRenderer;
    private VehicleController vehicleController;
    private ResourceManager resourceManager;

    private bool isRampPlaced = false;

    // Start is called before the first frame update
    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        vehicleController = FindObjectOfType<VehicleController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRampPlaced)
        {
            if (vehicleController.IsAirborne())
            {
                isRampPlaced = false;
            }
        }
        else
        {
            if (vehicleController.IsGrounded())
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
                }
                else
                {
                    transform.position = vehicleController.transform.position + Vector3.forward * ghostOffset;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRamp : MonoBehaviour
{
    [SerializeField] float ghostOffset;
    [SerializeField] GameObject[] rampPrefabs;

    private MeshRenderer meshRenderer;
    private VehicleController vehicleController;

    private bool isRampPlaced = false;

    // Start is called before the first frame update
    void Start()
    {
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
                GameObject rampPrefab = null;

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    rampPrefab = rampPrefabs[0];
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    rampPrefab = rampPrefabs[1];
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    rampPrefab = rampPrefabs[2];
                }

                if (rampPrefab != null)
                {
                    GameObject ramp = Instantiate(rampPrefab, transform.position, Quaternion.identity);
                    Destroy(ramp, 10);

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

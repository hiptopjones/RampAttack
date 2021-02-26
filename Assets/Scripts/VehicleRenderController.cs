using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleRenderController : MonoBehaviour
{
    // Collisions
    [SerializeField] GameObject voxelPrefab;
    [SerializeField] float explosionScatter = 1;
    [SerializeField] float numExplosionVoxels = 20;

    private VehiclePhysicsController vehiclePhysicsController;

    // Start is called before the first frame update
    void Start()
    {
        vehiclePhysicsController = FindObjectOfType<VehiclePhysicsController>();
        if (vehiclePhysicsController == null)
        {
            throw new System.Exception($"Unable to find {nameof(VehiclePhysicsController)}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (vehiclePhysicsController == null)
        {
            return;
        }

        transform.position = vehiclePhysicsController.transform.position;
        transform.rotation = vehiclePhysicsController.transform.rotation;
    }

    public void OnPlayerDeath()
    {
        for (int i = 0; i < numExplosionVoxels; i++)
        {
            Vector3 particlePosition = transform.position + new Vector3(Random.Range(-explosionScatter, explosionScatter), Random.Range(0, explosionScatter), Random.Range(-explosionScatter, explosionScatter));
            Vector3 eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            Instantiate(voxelPrefab, particlePosition, Quaternion.Euler(eulerAngles));
        }

        // TODO: Who should be reporting this?
        GameSession gameSession = FindObjectOfType<GameSession>();
        gameSession.PlayerDied();

        Destroy(gameObject);
    }
}

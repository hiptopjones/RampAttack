using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelManager : MonoBehaviour
{
    [SerializeField] float maxFuel;
    [SerializeField] float fuelBurnSpeed;

    private GameSession gameSession;
    private VehicleRenderController vehicleRenderController;
    private VehiclePhysicsController vehiclePhysicsController;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        if (gameSession == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(GameSession)}");
        }

        vehicleRenderController = FindObjectOfType<VehicleRenderController>();
        if (vehicleRenderController == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(VehicleRenderController)}");
        }

        vehiclePhysicsController = FindObjectOfType<VehiclePhysicsController>();
        if (vehiclePhysicsController == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(VehiclePhysicsController)}");
        }

        gameSession.SetMaxFuel(maxFuel);
        gameSession.SetCurrentFuel(maxFuel);
    }

    // Update is called once per frame
    void Update()
    {
        if (vehiclePhysicsController == null || vehicleRenderController == null)
        {
            return;
        }

        // Don't burn fuel if physics isn't running
        if (false == vehiclePhysicsController.IsRunning())
        {
            return;
        }

        float currentFuel = gameSession.GetCurrentFuel() - Time.deltaTime * fuelBurnSpeed;
        gameSession.SetCurrentFuel(currentFuel);

        if (currentFuel < 0)
        {
            vehicleRenderController.OnPlayerDeath();
        }
    }
}

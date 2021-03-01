using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelManager : MonoBehaviour
{
    [SerializeField] float maxFuel;
    [SerializeField] float fuelBurnSpeed;

    private GameManager gameManager;
    private VehicleRenderController vehicleRenderController;
    private VehiclePhysicsController vehiclePhysicsController;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(GameManager)}");
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

        gameManager.SetMaxFuel(maxFuel);
        gameManager.SetCurrentFuel(maxFuel);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager == null || vehiclePhysicsController == null || vehicleRenderController == null)
        {
            return;
        }

        // Don't burn fuel if physics isn't running
        if (false == vehiclePhysicsController.IsRunning())
        {
            return;
        }

        float currentFuel = gameManager.GetCurrentFuel() - Time.deltaTime * fuelBurnSpeed;
        gameManager.SetCurrentFuel(currentFuel);

        // TODO: Should play a warning sound (fast beep?) shortly before we run out (maybe flash the UI display?)
        if (currentFuel < 0)
        {
            vehicleRenderController.OnPlayerDeath();
        }
    }
}

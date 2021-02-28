using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelManager : MonoBehaviour
{
    [SerializeField] float maxFuel;
    [SerializeField] float fuelBurnSpeed;

    private GameSession gameSession;
    private VehicleRenderController vehicleRenderController;

    private float startTime;

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

        gameSession.SetMaxFuel(maxFuel);
        gameSession.SetCurrentFuel(maxFuel);

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float currentFuel = gameSession.GetMaxFuel() - (Time.time - startTime) * fuelBurnSpeed;
        gameSession.SetCurrentFuel(currentFuel);

        if (currentFuel < 0)
        {
            vehicleRenderController.OnPlayerDeath();
        }
    }
}

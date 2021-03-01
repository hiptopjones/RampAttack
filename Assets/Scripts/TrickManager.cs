using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickManager : MonoBehaviour
{
    private GameManager gameManager;
    private VehicleRenderController vehicleRenderController;
    private VehiclePhysicsController vehiclePhysicsController;
    private CountdownBar countdownBar;

    private bool isCountdownActive;
    private bool isCountdownExpired;
    private float startCountdownTime;
    private float availableCountdownTime = 1.5f;

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

        countdownBar = FindObjectOfType<CountdownBar>();
        if (countdownBar == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(CountdownBar)}");
        }

        countdownBar.Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager == null || vehiclePhysicsController == null || vehicleRenderController == null)
        {
            return;
        }

        if (false == vehiclePhysicsController.IsRunning())
        {
            return;
        }

        if (vehiclePhysicsController.IsGrounded())
        {
            isCountdownActive = false;
            isCountdownExpired = false;
        }

        if (false == isCountdownActive && false == isCountdownExpired)
        {
            if (vehiclePhysicsController.IsAirborne())
            {
                isCountdownActive = true;
                startCountdownTime = Time.time;

                countdownBar.SetMaxTime(availableCountdownTime);
                countdownBar.SetCurrentTime(availableCountdownTime);
                countdownBar.Show();
            }
        }

        if (isCountdownActive)
        {
            float remainingCountdownTime = availableCountdownTime - (Time.time - startCountdownTime);
            countdownBar.SetCurrentTime(remainingCountdownTime);

            if (remainingCountdownTime <= 0)
            {
                isCountdownActive = false;
                isCountdownExpired = true;
                countdownBar.Hide();
            }
        }
    }
}

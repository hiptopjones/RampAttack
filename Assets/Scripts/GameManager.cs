using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] float timeScale = 1.5f;

    IntroDisplay introDisplay;
    VehiclePhysicsController vehiclePhysicsController;
    SceneLoader sceneLoader;

    private int currentTowers;
    private int currentCoins;
    private int bestTowers;
    private int bestCoins;

    private float maxFuel;
    private float currentFuel;

    void Start()
    {
        introDisplay = FindObjectOfType<IntroDisplay>();
        if (introDisplay == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(IntroDisplay)}");
        }

        vehiclePhysicsController = FindObjectOfType<VehiclePhysicsController>();
        if (vehiclePhysicsController == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(VehiclePhysicsController)}");
        }

        sceneLoader = FindObjectOfType<SceneLoader>();
        if (sceneLoader == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(SceneLoader)}");
        }

        Time.timeScale = timeScale;

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return StartCoroutine(introDisplay.ReadySetGo());
        
        vehiclePhysicsController.StartPhysics();
    }

    // TODO: Use a proper event for this
    public void PlayerDied()
    {
        sceneLoader.LoadGameWithDelay();
    }

    public int GetCurrentCoins()
    {
        return currentCoins;
    }

    public int GetBestCoins()
    {
        return bestCoins;
    }

    public void AddCoins(int numCoins)
    {
        currentFuel += numCoins;

        currentCoins += numCoins;
        if (currentCoins > bestCoins)
        {
            bestCoins = currentCoins;
        }
    }

    public int GetCurrentTowers()
    {
        return currentTowers;
    }

    public int GetBestTowers()
    {
        return bestTowers;
    }

    public void SetTowers(int numTowers)
    {
        currentTowers = numTowers;
        if (currentTowers > bestTowers)
        {
            bestTowers = currentTowers;
        }
    }

    public void SetCurrentFuel(float currentFuel)
    {
        this.currentFuel = currentFuel;
    }

    public float GetCurrentFuel()
    {
        return currentFuel;
    }

    public void SetMaxFuel(float maxFuel)
    {
        this.maxFuel = maxFuel;
    }

    public float GetMaxFuel()
    {
        return maxFuel;
    }

    public void ResetGame()
    {
        currentTowers = 0;
        currentCoins = 0;
        currentFuel = 0;
    }
}
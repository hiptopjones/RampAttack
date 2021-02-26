using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    private int currentTowers;
    private int currentCoins;
    private int bestTowers;
    private int bestCoins;

    void Awake()
    {
        int numThings = FindObjectsOfType<GameSession>().Length;
        if (numThings > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1.5f;
    }

    // TODO: Use a proper event for this
    public void IntroComplete()
    {
        // GameSession lives across scenes, so look for a new object each time
        VehiclePhysicsController vehiclePhysicsController = FindObjectOfType<VehiclePhysicsController>();
        vehiclePhysicsController.OnGameStarted();
    }

    // TODO: Use a proper event for this
    public void PlayerDied()
    {
        // GameSession lives across scenes, so look for a new object each time
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
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

    public void ResetGame()
    {
        currentTowers = 0;
        currentCoins = 0;
    }
}
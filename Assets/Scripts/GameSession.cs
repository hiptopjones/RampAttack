using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    int numTowers;
    int numCoins;

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

    // TODO: Use a proper event for this
    public void IntroComplete()
    {
        VehicleController vehicleController = FindObjectOfType<VehicleController>();
        vehicleController.StartDriving();
    }

    // TODO: Use a proper event for this
    public void PlayerDied()
    {
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        sceneLoader.LoadGameWithDelay();
    }

    public int GetNumCoins()
    {
        return numCoins;
    }

    public void AddCoins(int coinsToAdd)
    {
        this.numCoins += coinsToAdd;
    }

    public int GetNumTowers()
    {
        return numTowers;
    }

    public void SetTowers(int numTowers)
    {
        this.numTowers = numTowers;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    int totalCoins;

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

    private void Start()
    {
        IntroDisplay introDisplay = FindObjectOfType<IntroDisplay>();
        introDisplay.StartIntro();
    }

    // TODO: Use a proper event for this
    public void IntroComplete()
    {
        VehicleController vehicleController = FindObjectOfType<VehicleController>();
        vehicleController.StartDriving();
    }

    public void PlayerDied()
    {
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        sceneLoader.LoadGameOverWithDelay();
    }

    public int GetTotalCoins()
    {
        return totalCoins;
    }

    public void AddCoins(int numCoins)
    {
        totalCoins += numCoins;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
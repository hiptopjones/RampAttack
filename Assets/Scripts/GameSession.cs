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
        ReadySetGoDisplay introDisplay = FindObjectOfType<ReadySetGoDisplay>();
        introDisplay.StartIntro();
    }

    // TODO: Use a proper event for this
    public void IntroComplete()
    {
        DriveController driveController = FindObjectOfType<DriveController>();
        driveController.StartDriving();
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
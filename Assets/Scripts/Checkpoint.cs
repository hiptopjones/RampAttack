using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(GameManager)}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PurchaseFuel();
        }
    }

    private void PurchaseFuel()
    {
        int currentCoins = gameManager.GetCurrentCoins();
        float currentFuel = gameManager.GetCurrentFuel();

        currentFuel += currentCoins;

        gameManager.SetCurrentFuel(currentFuel);
        gameManager.SetCurrentCoins(0);
    }
}

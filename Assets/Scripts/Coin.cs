using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;

    ResourceManager resourceManager;

    private void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
    }

    void Update()
    {
        transform.Rotate(transform.up, 360 * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameSession gameSession = FindObjectOfType<GameSession>();
            gameSession.AddCoins(1);

            GameObject coinEffect = resourceManager.GetOrCreateCoinEffect();
            coinEffect.transform.position = transform.position;

            resourceManager.DestroyCoin(gameObject);
        }
    }
}

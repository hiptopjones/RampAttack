using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float explosionTimeToLive = 2;

    void Update()
    {
        transform.Rotate(transform.up, 360 * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameSession gameSession = FindObjectOfType<GameSession>();
        gameSession.AddCoins(1);

        GameObject particleEffect = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(particleEffect, explosionTimeToLive);

        Destroy(gameObject);
    }
}

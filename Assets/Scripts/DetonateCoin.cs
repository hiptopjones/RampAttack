using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonateCoin : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float explosionTimeToLive = 2;



    private void OnTriggerEnter(Collider other)
    {
    

        GameObject particleEffect = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(particleEffect, explosionTimeToLive);

        Destroy(gameObject);
    }
}

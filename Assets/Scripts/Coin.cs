using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] float collectionTime = 1f;

    private ResourceManager resourceManager;
    private GameManager gameManager;
    private CoinCollector coinCollector;

    [SerializeField] bool isBeingCollected;
    [SerializeField] float startTime;
    [SerializeField] Vector3 startPosition;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(GameManager)}");
        }

        resourceManager = FindObjectOfType<ResourceManager>();
        if (resourceManager == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(ResourceManager)}");
        }

        coinCollector = FindObjectOfType<CoinCollector>();
        if (coinCollector == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(CoinCollector)}");
        }
    }

    void Update()
    {
        transform.Rotate(transform.up, 360 * rotationSpeed * Time.deltaTime);

        if (isBeingCollected)
        {
            // This changes every frame, as the collector keeps up with the player
            Vector3 endPosition = coinCollector.transform.position;

            // TODO: Use an animation curve
            float deltaPercent = (Time.time - startTime) / collectionTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, deltaPercent);

            float remainingDistance = Mathf.Abs((endPosition - transform.position).magnitude);
            if (remainingDistance < 1)
            {
                isBeingCollected = false;

                gameManager.AddCoins(1);
                resourceManager.DestroyCoin(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isBeingCollected = true;
            startTime = Time.time;
            startPosition = transform.position;
        }
    }
}

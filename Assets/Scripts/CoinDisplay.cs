using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    [SerializeField] bool isBest;

    private TextMeshProUGUI coinText;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        coinText = GetComponent<TextMeshProUGUI>();
        if (coinText == null)
        {
            throw new System.Exception($"Unable to get component of type {nameof(TextMeshProUGUI)}");
        }

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(GameManager)}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = isBest ? gameManager.GetBestCoins().ToString(): gameManager.GetCurrentCoins().ToString();
    }
}

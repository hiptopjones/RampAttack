using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    [SerializeField] bool isBest;

    private TextMeshProUGUI coinText;
    private GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        coinText = GetComponent<TextMeshProUGUI>();
        if (coinText == null)
        {
            throw new System.Exception($"Unable to get component of type {nameof(TextMeshProUGUI)}");
        }

        gameSession = FindObjectOfType<GameSession>();
        if (gameSession == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(GameSession)}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = isBest ? gameSession.GetBestCoins().ToString(): gameSession.GetCurrentCoins().ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    TextMeshProUGUI coinText;
    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        coinText = GetComponent<TextMeshProUGUI>();
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = gameSession.GetTotalCoins().ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerDisplay : MonoBehaviour
{
    [SerializeField] bool isBest;

    TextMeshProUGUI towerText;
    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        towerText = GetComponent<TextMeshProUGUI>();
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        towerText.text = isBest ? gameSession.GetBestTowers().ToString() : gameSession.GetCurrentTowers().ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerDisplay : MonoBehaviour
{
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
        towerText.text = gameSession.GetNumTowers().ToString();
    }
}

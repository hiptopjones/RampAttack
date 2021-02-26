using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerDisplay : MonoBehaviour
{
    [SerializeField] bool isBest;

    private TextMeshProUGUI towerText;
    private GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        towerText = GetComponent<TextMeshProUGUI>();
        if (towerText == null)
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
        towerText.text = isBest ? gameSession.GetBestTowers().ToString() : gameSession.GetCurrentTowers().ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerDisplay : MonoBehaviour
{
    private TextMeshProUGUI towerText;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        towerText = GetComponent<TextMeshProUGUI>();
        if (towerText == null)
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
        towerText.text = gameManager.GetCurrentTowers().ToString();
    }
}

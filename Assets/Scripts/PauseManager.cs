using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pausedText;

    bool isPaused = false;

    private void Start()
    {
        pausedText.SetText(string.Empty);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        if (isPaused)
        {
            isPaused = false;
            pausedText.SetText(string.Empty);
            Time.timeScale = 1;
        }
        else
        {
            isPaused = true;
            pausedText.SetText("-PAUSED-");
            Time.timeScale = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroDisplay : MonoBehaviour
{
    [SerializeField] float textInterval = 0.5f;

    [SerializeField] int startFontSize;
    [SerializeField] int endFontSize;

    private GameSession gameSession;
    private TextMeshProUGUI introText;
    private float textStartTime;

    void Start()
    {
        introText = GetComponent<TextMeshProUGUI>();
        if (introText == null)
        {
            throw new System.Exception($"Unable to get component of type {nameof(TextMeshProUGUI)}");
        }

        gameSession = FindObjectOfType<GameSession>();
        if (gameSession == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(GameSession)}");
        }

        StartCoroutine(ReadySetGo());
    }

    void Update()
    {
        float currentFontSize = Mathf.Lerp(startFontSize, endFontSize, Time.time - textStartTime);
        introText.fontSize = currentFontSize;
    }

    private IEnumerator ReadySetGo()
    {
        SetText("Ready!");
        yield return new WaitForSeconds(textInterval);
        SetText("Set!");
        yield return new WaitForSeconds(textInterval);
        SetText("Go!");
        yield return new WaitForSeconds(textInterval);
        SetText(string.Empty);

        // Let the session manager know we're done
        gameSession.IntroComplete();
    }

    private void SetText(string text)
    {
        introText.text = text;
        introText.fontSize = startFontSize;

        textStartTime = Time.time;
    }
}

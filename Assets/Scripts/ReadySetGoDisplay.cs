using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReadySetGoDisplay : MonoBehaviour
{
    [SerializeField] float textInterval = 0.5f;

    [SerializeField] int startFontSize;
    [SerializeField] int endFontSize;

    GameSession gameSession;
    TextMeshProUGUI introText;
    float textStartTime;

    // Start is called before the first frame update
    void Start()
    {
        introText = GetComponent<TextMeshProUGUI>();
        gameSession = FindObjectOfType<GameSession>();
    }

    void Update()
    {
        float currentFontSize = Mathf.Lerp(startFontSize, endFontSize, Time.time - textStartTime);
        introText.fontSize = currentFontSize;
    }

    public void StartIntro()
    {
        StartCoroutine(ReadySetGo());
    }

    private IEnumerator ReadySetGo()
    {
        SetText("Ready!");
        yield return new WaitForSeconds(textInterval);
        SetText("Set!");
        yield return new WaitForSeconds(textInterval);
        SetText("Go!");
        yield return new WaitForSeconds(textInterval);
        SetText("ALL DONE!");

        // Let the session manager know we're done
        gameSession.IntroComplete();

        Destroy(gameObject);
    }

    private void SetText(string text)
    {
        introText.text = text;
        introText.fontSize = startFontSize;

        textStartTime = Time.time;
    }
}

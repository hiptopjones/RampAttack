using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroDisplay : MonoBehaviour
{
    [SerializeField] float textInterval = 0.5f;

    [SerializeField] int startFontSize;
    [SerializeField] int endFontSize;

    private TextMeshProUGUI introText;

    private float textStartTime;

    void Start()
    {
        introText = GetComponent<TextMeshProUGUI>();
        if (introText == null)
        {
            throw new System.Exception($"Unable to get component of type {nameof(TextMeshProUGUI)}");
        }
    }

    void Update()
    {
        if (false == introText.enabled)
        {
            return;
        }

        
        float currentFontSize = Mathf.Lerp(startFontSize, endFontSize, (Time.time - textStartTime) / textInterval);
        introText.fontSize = currentFontSize;
    }

    public IEnumerator ReadySetGo()
    {
        // If the coroutine is running before Start() got called, just wait longer
        if (introText == null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        introText.enabled = true;

        SetText("Ready!");
        yield return new WaitForSeconds(textInterval);
        SetText("Set!");
        yield return new WaitForSeconds(textInterval);
        SetText("Go!");
        yield return new WaitForSeconds(textInterval);
        SetText(string.Empty);

        introText.enabled = false;
    }

    private void SetText(string text)
    {
        introText.text = text;
        introText.fontSize = startFontSize;

        textStartTime = Time.time;
    }
}

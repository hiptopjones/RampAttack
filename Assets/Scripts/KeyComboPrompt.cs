using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyComboPrompt : MonoBehaviour
{
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;

    private Slider slider;
    private TextMeshProUGUI keyComboText;

    void Start()
    {
        Show();

        slider = GetComponentInChildren<Slider>();
        if (slider == null)
        {
            throw new System.Exception($"Unable to get component in children of type {nameof(Slider)}");
        }

        keyComboText = GetComponentInChildren<TextMeshProUGUI>();
        if (keyComboText == null)
        {
            throw new System.Exception($"Unable to get component in children of type {nameof(TextMeshProUGUI)}");
        }

        Hide();
    }

    public void SetMaxTime(float maxTime)
    {
        slider.maxValue = maxTime;
    }

    public void SetCurrentTime(float currentTime)
    {
        slider.value = currentTime;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetKeyComboText(string keyCombo)
    {
        keyComboText.text = keyCombo;
    }

    public void Show()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}

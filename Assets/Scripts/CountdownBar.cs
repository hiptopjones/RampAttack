using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownBar : MonoBehaviour
{
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;

    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        if (slider == null)
        {
            throw new System.Exception($"Unable to get component of type {nameof(Slider)}");
        }
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
}

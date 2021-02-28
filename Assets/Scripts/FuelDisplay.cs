using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelDisplay : MonoBehaviour
{
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;

    private Slider slider;
    private GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        if (slider == null)
        {
            throw new System.Exception($"Unable to get component of type {nameof(Slider)}");
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
        slider.maxValue = gameSession.GetMaxFuel();
        slider.value = gameSession.GetCurrentFuel();
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}

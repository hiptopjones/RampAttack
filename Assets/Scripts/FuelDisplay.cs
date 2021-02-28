using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelDisplay : MonoBehaviour
{
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;

    private Slider slider;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        if (slider == null)
        {
            throw new System.Exception($"Unable to get component of type {nameof(Slider)}");
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
        slider.maxValue = gameManager.GetMaxFuel();
        slider.value = gameManager.GetCurrentFuel();
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}

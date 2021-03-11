using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelDisplay : MonoBehaviour
{
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    [SerializeField] float flashPeriod;

    private Slider slider;
    private GameManager gameManager;

    private bool isFlashEnabled;

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
        float maxFuel = gameManager.GetMaxFuel();
        float currentFuel = gameManager.GetCurrentFuel();

        slider.maxValue = maxFuel;
        slider.value = currentFuel;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        
        float fuelPercent = currentFuel / maxFuel;
        if (fuelPercent < 0.2f)
        {
            isFlashEnabled = true;
            StartCoroutine(Flash());
        }
        else
        {
            isFlashEnabled = false;
        }
    }

    private IEnumerator Flash()
    {
        int alpha = 0;

        while (true)
        {
            if (false == isFlashEnabled)
            {
                yield break;
            }

            fill.color = new Color(fill.color.r, fill.color.g, fill.color.b, alpha);
            alpha = Mathf.Abs(alpha - 1);

            yield return new WaitForSeconds(flashPeriod / 2);
        }
    }
}

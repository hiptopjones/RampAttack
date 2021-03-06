using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrickManager : MonoBehaviour
{
    private GameManager gameManager;
    private VehicleRenderController vehicleRenderController;
    private VehiclePhysicsController vehiclePhysicsController;
    private KeyComboPrompt keyComboPrompt;

    private bool isCountdownActive;
    private bool isCountdownExpired;
    private float startCountdownTime;
    private float availableCountdownTime = 1.5f;

    private int currentKeyComboIndex;
    private KeyCode[] keyCombo;

    private KeyCode[][] keyCombos = new[]
    {
        new [] {KeyCode.LeftArrow, KeyCode.RightArrow },
        new [] {KeyCode.UpArrow, KeyCode.DownArrow },
        new [] {KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.RightArrow },
        new [] {KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow }
    };

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(GameManager)}");
        }

        vehicleRenderController = FindObjectOfType<VehicleRenderController>();
        if (vehicleRenderController == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(VehicleRenderController)}");
        }

        vehiclePhysicsController = FindObjectOfType<VehiclePhysicsController>();
        if (vehiclePhysicsController == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(VehiclePhysicsController)}");
        }

        keyComboPrompt = FindObjectOfType<KeyComboPrompt>();
        if (keyComboPrompt == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(KeyComboPrompt)}");
        }

        keyComboPrompt.Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager == null || vehiclePhysicsController == null || vehicleRenderController == null)
        {
            return;
        }

        if (false == vehiclePhysicsController.IsRunning())
        {
            return;
        }

        if (vehiclePhysicsController.IsGrounded())
        {
            isCountdownActive = false;
            isCountdownExpired = false;
        }

        if (false == isCountdownActive && false == isCountdownExpired)
        {
            if (vehiclePhysicsController.IsAirborne())
            {
                isCountdownActive = true;
                startCountdownTime = Time.time;

                currentKeyComboIndex = 0;

                keyCombo = keyCombos[(int)(Random.value * keyCombos.Length)];

                keyComboPrompt.SetKeyComboText(GetKeyComboText(keyCombo));
                keyComboPrompt.SetMaxTime(availableCountdownTime);
                keyComboPrompt.SetCurrentTime(availableCountdownTime);
                keyComboPrompt.Show();
            }
        }

        if (isCountdownActive)
        {
            KeyCode nextKeyCode = keyCombo[currentKeyComboIndex];
            if (Input.GetKeyDown(nextKeyCode))
            {
                currentKeyComboIndex++;
                if (currentKeyComboIndex >= keyCombo.Length)
                {
                    DoTrick();

                    isCountdownActive = false;
                    isCountdownExpired = true;

                    keyComboPrompt.Hide();
                }
            }
            else if (Input.anyKeyDown)
            {
                // Reset key combo if any other key was pressed
                currentKeyComboIndex = 0;
            }

            float remainingCountdownTime = availableCountdownTime - (Time.time - startCountdownTime);
            keyComboPrompt.SetCurrentTime(remainingCountdownTime);

            if (remainingCountdownTime <= 0)
            {
                isCountdownActive = false;
                isCountdownExpired = true;

                keyComboPrompt.Hide();
            }
        }
    }

    private string GetKeyComboText(KeyCode[] keyCombo)
    {
        return string.Join(" ", keyCombo.Select(x => GetKeyCodeText(x)));
    }

    private string GetKeyCodeText(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.UpArrow:
                return "Up";

            case KeyCode.DownArrow:
                return "Down";

            case KeyCode.LeftArrow:
                return "Left";

            case KeyCode.RightArrow:
                return "Right";

            default:
                return keyCode.ToString();
        }
    }

    private void DoTrick()
    {
        vehicleRenderController.StartRoll();
    }
}

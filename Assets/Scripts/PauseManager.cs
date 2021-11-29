using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseManager : MonoBehaviour
{
    private float savedTimeScale;

    private SceneLoader sceneLoader;

    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        if (sceneLoader == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(SceneLoader)}");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (savedTimeScale == 0)
            {
                savedTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = savedTimeScale;
                savedTimeScale = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            sceneLoader.LoadGameOver();
        }
    }
}

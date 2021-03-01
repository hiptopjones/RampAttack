using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseManager : MonoBehaviour
{
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
        // TODO: Should just pause the game, not end it
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            sceneLoader.LoadGameOver();
        }
    }
}

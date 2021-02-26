using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] float gameDelaySeconds = 2;

    public void LoadGameOver()
    {
        SceneManager.LoadScene("Game Over");
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void LoadGameWithDelay()
    {
        StartCoroutine(LoadGameWithDelayCoroutine());
    }

    public IEnumerator LoadGameWithDelayCoroutine()
    {
        yield return new WaitForSeconds(gameDelaySeconds);
        LoadGame();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");

        // GameSession lives across scenes, so look for a new object each time
        GameSession gameSession = FindObjectOfType<GameSession>();
        if (gameSession != null)
        {
            gameSession.ResetGame();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

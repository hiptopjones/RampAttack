using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] float gameOverDelaySeconds = 2;

    public void LoadGameOver()
    {
        StartCoroutine(LoadGameOverWithDelay());
    }

    public IEnumerator LoadGameOverWithDelay()
    {
        yield return new WaitForSeconds(gameOverDelaySeconds);
        SceneManager.LoadScene("Game Over");
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePausedUI : MonoBehaviour
{

    public GameObject pauseUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameEnded)
        {
            enabled = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        pauseUI.SetActive(!pauseUI.activeSelf);

        // Freeze time if the pause menu is paused
        if (pauseUI.activeSelf)
        {
            Time.timeScale = 0f;
            Time.fixedDeltaTime = 0f;
        } else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 1f;
        }
    }

    public void Retry()
    {
        TogglePauseMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        TogglePauseMenu();
        SceneManager.LoadScene("MainMenu");
    }
}

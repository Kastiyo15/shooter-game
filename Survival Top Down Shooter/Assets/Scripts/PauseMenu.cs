using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool _dead = false;

    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private GameObject _deathMenuUI;

    [SerializeField] private string _mainMenu;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_dead)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }


    public void Resume()
    {
        // Game Properties
        _pauseMenuUI.SetActive(false);
        _deathMenuUI.SetActive(false);

        Time.timeScale = 1f;
        GameIsPaused = false;
        _dead = false;

        // Cursor Properties
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }


    void Pause()
    {
        // Game Properties
        _pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        GameIsPaused = true;

        // Cursor Properties
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void DeathMenu()
    {
        // Game Properties
        _deathMenuUI.SetActive(true);
        _dead = true;

        Time.timeScale = 1f;
        GameIsPaused = true;

        // Cursor Properties
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void Restart()
    {
        Debug.Log("Restarting...");
        _dead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void LoadMenu()
    {
        Debug.Log("Loading Menu...");
        SceneManager.LoadScene(_mainMenu);
    }


    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}

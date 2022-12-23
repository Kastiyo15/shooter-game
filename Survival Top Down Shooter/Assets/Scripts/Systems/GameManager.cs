using UnityEngine;
using System;
using System.Collections;


public class GameManager : MonoBehaviour
{

    public GameObject Player01;
    [SerializeField] private GameObject _spawnerParent;
    [SerializeField] private PauseMenu _deathScreen;
    [SerializeField] private GameObject _UICamera;

    // Stopwatch
    private bool _timerActive = false;
    public float CurrentTime;
    public TimeSpan Timer;



    private void Awake()
    {
        Player01.SetActive(true);
        _UICamera.SetActive(true);
        CurrentTime = 0f;
        _timerActive = true;
        PauseMenu._dead = false;
    }


    private void Update()
    {
        // Inactivate spawner if player dies
        if (!Player01.activeInHierarchy)
        {
            _spawnerParent.SetActive(false);
            _UICamera.SetActive(false);
            StartCoroutine(StartDeathMenu());
        }

        // Start Stopwatch when game isn't paused and player isn't dead
        if (!PauseMenu.GameIsPaused && Player01.activeInHierarchy)
        {
            _timerActive = true;
        }
        else
        {
            _timerActive = false;
        }

        // When timer active, add to the time value
        if (_timerActive)
        {
            CurrentTime += Time.deltaTime;
        }

        // Create a timespan for use in the Deathscreen Stats Script
        Timer = TimeSpan.FromSeconds(CurrentTime);
    }


    private IEnumerator StartDeathMenu()
    {
        _timerActive = false;
        yield return new WaitForSeconds(1f);
        _deathScreen.GetComponent<PauseMenu>().DeathMenu();
    }
}


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class GameManager : MonoBehaviour, ISaveable
{

    [Header("References")]
    [SerializeField] private GameObject _spawnerParent;
    [SerializeField] private PauseMenu _deathScreen;
    [SerializeField] private GameObject _UICamera;
    public GameObject Player01;


    [Header("Stats For DeathScreen")]
    [SerializeField] public int TotalDamage = 0;
    [SerializeField] public int BulletsFired = 0;


    // Stopwatch
    private bool _timerActive = false;
    public float CurrentTime;
    public TimeSpan Timer;

    private bool once = true;



    private void Awake()
    {
        // Load Scores
        LoadJsonData(this);

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

            if (once)
            {
                StopCoroutine(StartDeathMenu());
                StartCoroutine(StartDeathMenu());
                once = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Player01.GetComponent<Health>().Kill();
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
        if (_timerActive)
        {
            // Add damage and bullets to relative total scores and then the total score
            ScoreManager.Instance.BulletsFiredScore = BulletsFired;
            ScoreManager.Instance.TotalDamageScore = TotalDamage;

            // Add death score
            ScoreManager.Instance.DeathScore();
        }
        _timerActive = false;

        yield return new WaitForSeconds(1f);

        // Save Game Data
        //SaveJsonData(this);

        _deathScreen.GetComponent<PauseMenu>().DeathMenu();
    }



    //////////////////////////////////////////////////////////////////
    // SAVING AND LOADING //
    public static void SaveJsonData(GameManager a_GameManager)
    {
        SaveData sd = new SaveData();
        a_GameManager.PopulateSaveData(sd);

        if (FileManager.WriteToFile("SaveData.dat", sd.ToJson()))
        {
            Debug.Log("Save Successful");
        }
    }


    public void PopulateSaveData(SaveData a_SaveData)
    {
        // Save Career Stats
        a_SaveData.m_CareerBullets = CareerStats.Instance.CareerBullets;
        a_SaveData.m_CareerDamage = CareerStats.Instance.CareerDamage;
        a_SaveData.m_CareerKills = CareerStats.Instance.CareerKills;

        // Save Player Level Data
        LevelSystem.Instance.PopulateSaveData(a_SaveData);

        // Save Player Score Data
        ScoreManager.Instance.PopulateSaveData(a_SaveData);

        // Save data from statsmanager
        StatsManager.Instance.PopulateSaveData(a_SaveData);
    }



    public static void LoadJsonData(GameManager a_GameManager)
    {
        if (FileManager.LoadFromFile("SaveData.dat", out var json))
        {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);

            a_GameManager.LoadFromSaveData(sd);
            Debug.Log("Load Complete");
        }
    }



    public void LoadFromSaveData(SaveData a_SaveData)
    {
        // Load Career Stats
        CareerStats.Instance.CareerBullets = a_SaveData.m_CareerBullets;
        CareerStats.Instance.CareerDamage = a_SaveData.m_CareerDamage;
        CareerStats.Instance.CareerKills = a_SaveData.m_CareerKills;

        // Load Player Level Data
        LevelSystem.Instance.LoadFromSaveData(a_SaveData);

        // Save Player Score Data
        ScoreManager.Instance.LoadFromSaveData(a_SaveData);

        // Load data into statsmanager
        StatsManager.Instance.LoadFromSaveData(a_SaveData);

    }
    //////////////////////////////////////////////////////////////////
}


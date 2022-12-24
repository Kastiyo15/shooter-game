using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


public class DeathScreenStats : MonoBehaviour
{
    [Header("Stats Text")]
    [SerializeField] private TMP_Text _totalDamage;
    [SerializeField] private TMP_Text _bulletsFired;
    [SerializeField] private TMP_Text _totalDamageXP;
    [SerializeField] private TMP_Text _bulletsFiredXP;
    [SerializeField] private TMP_Text _timeAlive;
    [SerializeField] private TMP_Text _enemiesKilled;
    [SerializeField] private TMP_Text _timeAliveXP;
    [SerializeField] private TMP_Text _enemiesKilledXP;
    [SerializeField] private TMP_Text _totalXP;

    [Header("References")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private float _timer;
    [SerializeField] private float _tmpTimer;

    private TimeSpan _storedTime;


    // Start is called before the first frame update
    void Start()
    {
        _tmpTimer = 0f;
    }


    private void Update()
    {
        _timer += Time.deltaTime;

        var killCount = Mathf.Lerp(0, KillCounter.FindObjectOfType<KillCounter>().KillCount, _timer / 2);

        // Exp Text
        var sManager = ScoreManager.Instance;

        var totalDamage = Mathf.Lerp(0, _gameManager.GetComponent<GameManager>().TotalDamage, _timer / 2);
        var bulletsFired = Mathf.Lerp(0, _gameManager.GetComponent<GameManager>().BulletsFired, _timer / 2);
        var damageScore = Mathf.Lerp(0, sManager.TotalDamageScore, _timer / 2);
        var bulletScore = Mathf.Lerp(0, sManager.BulletsFiredScore, _timer / 2);

        var aliveScore = Mathf.Lerp(0, sManager.TotalAliveScore, _timer / 2);
        var killScore = Mathf.Lerp(0, sManager.TotalKillScore, _timer / 2);
        var totalScore = Mathf.Lerp(0, sManager.Score, _timer / 2);

        TimerDisplay();
        UpdateUI(totalDamage, bulletsFired, damageScore, bulletScore, killCount, aliveScore, killScore, totalScore);
    }


    // Set text on all ui elements
    private void UpdateUI(float A, float B, float C, float D, float E, float F, float G, float H)
    {
        _timeAlive.text = _storedTime.ToString(@"mm\:ss\:ff");

        _totalDamage.SetText($"{A:#0}");
        _bulletsFired.SetText($"{B:#0}");
        _totalDamageXP.SetText($"+{C:#0}");
        _bulletsFiredXP.SetText($"+{D:#0}");

        _enemiesKilled.SetText($"{E:#0}");
        _timeAliveXP.SetText($"+{F:#0}");
        _enemiesKilledXP.SetText($"+{G:#0}");
        _totalXP.SetText($"+{H:#0}");
    }


    // Use same method as GameManager script, to animate the time in the deathscreen
    private void TimerDisplay()
    {
        // Run from 0 to how long the timer ran for
        if (_tmpTimer < _gameManager.CurrentTime)
        {
            _tmpTimer = Mathf.Lerp(0, _gameManager.CurrentTime, _timer / 2);
        }

        _storedTime = TimeSpan.FromSeconds(_tmpTimer);
    }
}

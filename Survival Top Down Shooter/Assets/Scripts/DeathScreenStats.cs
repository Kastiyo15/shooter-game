using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


public class DeathScreenStats : MonoBehaviour
{
    [Header("Stats Text")]
    [SerializeField] private TMP_Text _timeAlive;
    [SerializeField] private TMP_Text _enemiesKilled;
    [SerializeField] private TMP_Text _timeAliveXP;
    [SerializeField] private TMP_Text _enemiesKilledXP;
    [SerializeField] private TMP_Text _totalXP;

    [Header("References")]
    [SerializeField] private GameManager _stopwatch;


    private float _timer;



    // Start is called before the first frame update
    void Start()
    {


        // Stats text
        var currentTime = _stopwatch.GetComponent<GameManager>().Timer;
        _timeAlive.text = (currentTime.ToString(@"mm\:ss\:ff"));
        //_enemiesKilled.text = (KillCounter.FindObjectOfType<KillCounter>().KillCount).ToString();

    }


    private void Update()
    {
        _timer += Time.deltaTime;

        var killCount = Mathf.Lerp(0, KillCounter.FindObjectOfType<KillCounter>().KillCount, _timer / 2);

        // Exp Text
        var sManager = ScoreManager.Instance;
        var aliveScore = Mathf.Lerp(0, sManager.TotalAliveScore, _timer / 2);
        var killScore = Mathf.Lerp(0, sManager.TotalKillScore, _timer / 2);
        var totalScore = Mathf.Lerp(0, sManager.Score, _timer / 2);

        UpdateUI(killCount, aliveScore, killScore, totalScore);
    }


    private void UpdateUI(float B, float C, float D, float E)
    {
        _enemiesKilled.SetText($"{B:#0}");
        _timeAliveXP.SetText($"+{C:#0}");
        _enemiesKilledXP.SetText($"+{D:#0}");
        _totalXP.SetText($"+{E:#0}");
    }
}

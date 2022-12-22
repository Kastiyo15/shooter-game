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



    // Start is called before the first frame update
    void Start()
    {
        // Shortening the code
        var sManager = ScoreManager.Instance;
        var currentTime = _stopwatch.GetComponent<GameManager>().Timer;

        // Stats text
        _timeAlive.text = (currentTime.ToString(@"mm\:ss\:ff"));
        _enemiesKilled.text = (KillCounter.FindObjectOfType<KillCounter>().KillCount).ToString();

        // Exp Text
        _timeAliveXP.text = (sManager.TotalAliveScore).ToString();
        _enemiesKilledXP.text = (sManager.TotalKillScore).ToString();
        _totalXP.text = (sManager.Score).ToString();
    }
}

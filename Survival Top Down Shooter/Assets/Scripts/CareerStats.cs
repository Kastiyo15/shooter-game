using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class CareerStats : MonoBehaviour
{
    public static CareerStats Instance;

    [Header("Career Totals Text")]
    public float CareerDamage;
    public float CareerBullets;
    public float CareerKills;

    [Header("Career Totals Text")]
    [SerializeField] private TMP_Text _txtCareerDamage;
    [SerializeField] private TMP_Text _txtCareerBullets;
    [SerializeField] private TMP_Text _txtCareerKills;

    [Header("New Scores")]
    [SerializeField] private TMP_Text _newDamage;
    [SerializeField] private TMP_Text _newBullets;
    [SerializeField] private TMP_Text _newKills;

    [Header("References")]
    [SerializeField] private DeathScreenStats _deathStats;
    [SerializeField] private CanvasGroup _thisPage;

    // temporary variables for storing career stats
    private float _txtDamage;
    private float _txtBullets;
    private float _txtKills;
    private float _timer;


    void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        _deathStats.GetComponent<DeathScreenStats>();

        // Set the values of the career texts, before we lerp the values from the new scores
        //UpdateUI();

        _timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Store values of career stats in a new temp variable
        // minus the new score from career score
        // lerp add the new score to the career score
        // and lerp minus new score until 0

        if (_thisPage.alpha == 1)
        {
            _timer += Time.deltaTime;

            // Lerp career stats
            _txtDamage = Mathf.Lerp(CareerDamage - _deathStats.totalDamage, CareerDamage, _timer * 1f);
            _txtBullets = Mathf.Lerp(CareerBullets - _deathStats.bulletsFired, CareerBullets, _timer * 0.5f);
            _txtKills = Mathf.Lerp(CareerKills - _deathStats.killCount, CareerKills, _timer * 0.35f);

            // Lerp this run stats
            var txtNewDamage = Mathf.Lerp(_deathStats.totalDamage, 0, _timer * 1f);
            var txtNewBullets = Mathf.Lerp(_deathStats.bulletsFired, 0, _timer * 0.5f);
            var txtNewKills = Mathf.Lerp(_deathStats.killCount, 0, _timer * 0.35f);


            // Set text function
            UpdateUI(_txtDamage, _txtBullets, _txtKills, txtNewDamage, txtNewBullets, txtNewKills);

        }
        else if (_thisPage.alpha < 1)
        {
            UpdateUI(CareerDamage - _deathStats.totalDamage, CareerBullets - _deathStats.bulletsFired, CareerKills - _deathStats.killCount, _deathStats.totalDamage, _deathStats.bulletsFired, _deathStats.killCount);
        }
    }


    private void UpdateUI(float A, float B, float C, float D, float E, float F)
    {
        // Set the values of the career texts, before we lerp the values from the new scores
        _txtCareerDamage.SetText($"{A:#0}");
        _txtCareerBullets.SetText($"{B:#0}");
        _txtCareerKills.SetText($"{C:#0}");

        // Set the values of the 'this run' text, taken from the previous screen
        _newDamage.SetText($"+{D:#0}");
        _newBullets.SetText($"+{E:#0}");
        _newKills.SetText($"+{F:#0}");
    }


    // Update career stats page
    public void UpdateCareerStats()
    {
        // Add new score to the careers stats
        CareerDamage += _deathStats.totalDamage;
        CareerBullets += _deathStats.bulletsFired;
        CareerKills += _deathStats.killCount;
    }
}

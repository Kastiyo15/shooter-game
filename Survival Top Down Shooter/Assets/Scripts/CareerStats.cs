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


    void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        _deathStats.GetComponent<DeathScreenStats>();

        // Set the values of the career texts, before we lerp the values from the new scores
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }


    private void UpdateUI()
    {
        // Set the values of the career texts, before we lerp the values from the new scores
        _txtCareerDamage.SetText($"{CareerDamage:#0}");
        _txtCareerBullets.SetText($"{CareerBullets:#0}");
        _txtCareerKills.SetText($"{CareerKills:#0}");
    }


    // Update career stats page
    public void UpdateCareerStats()
    {
        CareerDamage += _deathStats.totalDamage;
        CareerBullets += _deathStats.bulletsFired;
        CareerKills += _deathStats.killCount;
    }
}

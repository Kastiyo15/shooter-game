using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class UpgradeBar : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("References")]
    [SerializeField] public int _iD;
    [SerializeField] private Image _fillBarBG; // This should lerp first
    [SerializeField] private Image _fillBarFG; // Then the foreground follows
    [SerializeField] private float _duration = 0.25f; // Time it takes to fill bar when clicked
    private TalentBarData _data;

    [Header("Stats")]
    [SerializeField] public int UniquePointsUsed = 1; // 1 for each bar filled: MAXIMUM 20 minimum 1
    [SerializeField] private int _pointsMaximum = 20; // Maximum points it takes to fill the bar
    [SerializeField] private float _fillValue = 0.05f; // How many pixels will be filled with each click

    private bool _cooldown = false;


    public void Start()
    {
        UniquePointsUsed = StatsManager.Instance.PointsSpentList[_iD];

        StartCoroutine(IncreaseBar());
    }


    // Function to increase points
    public void PlusButton()
    {
        // Start coroutine when points are less than maximum and cooldown is not active
        if (UniquePointsUsed < _pointsMaximum && _cooldown == false && StatsManager.Instance.AvailableTalentPoints > 0)
        {
            //UniquePointsUsed++;
            StatsManager.Instance.PointsSpentList[_iD]++;

            UpdateStatsManagerPoints(0);

            StartCoroutine(IncreaseBar());

            _cooldown = true;
        }
    }


    // Function to decrease points
    public void MinusButton()
    {
        if (UniquePointsUsed > 1 && _cooldown == false && StatsManager.Instance.AvailableTalentPoints != StatsManager.Instance.ls_TalentPoint)
        {
            //UniquePointsUsed--;
            StatsManager.Instance.PointsSpentList[_iD]--;

            UpdateStatsManagerPoints(1);

            StartCoroutine(DecreaseBar());

            _cooldown = true;
        }
    }


    // 
    private void UpdateStatsManagerPoints(int i)
    {
        if (i == 0)
        {
            StatsManager.Instance.UsedTalentPoints += 1;
        }
        else if (i == 1)
        {
            StatsManager.Instance.UsedTalentPoints -= 1;
        }
        StatsManager.Instance.SetLevelStats();
    }


    // Coroutine to adjust fill amount values, instead of using lerp in the Update function
    private IEnumerator IncreaseBar()
    {
        // Move the background fill instantly when points change
        _fillBarBG.fillAmount = StatsManager.Instance.PointsSpentList[_iD] * _fillValue;

        // Store the front fill amount in a local variable
        float FFA = _fillBarFG.fillAmount;

        // Increase fill amount for length stated in _duration
        for (float t = 0.0f; t < _duration; t += Time.deltaTime)
        {
            _fillBarFG.fillAmount = Mathf.Lerp(FFA, _fillBarBG.fillAmount, t / _duration);
            yield return null;
        }

        // Set front bar equal to back bar, to avoid weird numbers
        _fillBarFG.fillAmount = _fillBarBG.fillAmount;

        // Reset Cooldown
        _cooldown = false;
    }


    // Coroutine to adjust fill amount values, instead of using lerp in the Update function
    private IEnumerator DecreaseBar()
    {
        // Move the background fill instantly when points change
        _fillBarFG.fillAmount = StatsManager.Instance.PointsSpentList[_iD] * _fillValue;

        // Store the front fill amount in a local variable
        float BFA = _fillBarBG.fillAmount;

        // Increase fill amount for length stated in _duration
        for (float t = 0.0f; t < _duration; t += Time.deltaTime)
        {
            _fillBarBG.fillAmount = Mathf.Lerp(BFA, _fillBarFG.fillAmount, t / _duration);
            yield return null;
        }

        // Set front bar equal to back bar, to avoid weird numbers
        _fillBarBG.fillAmount = _fillBarFG.fillAmount;

        // Reset Cooldown
        _cooldown = false;
    }


    // Events using interfaces to check if the button is selected or not
    public void OnSelect(BaseEventData eventData)
    {
        //_infoPanel.SetText(_infoText);
    }


    public void OnDeselect(BaseEventData eventData)
    {
        //_infoPanel.SetText(_defaultText);
    }
}

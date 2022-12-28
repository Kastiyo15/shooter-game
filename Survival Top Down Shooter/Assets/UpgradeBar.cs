using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UpgradeBar : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("References")]
    [SerializeField] private Image _fillBarBG; // This should lerp first
    [SerializeField] private Image _fillBarFG; // Then the foreground follows
    [SerializeField] private float _duration = 0.25f; // Time it takes to fill bar when clicked
    [SerializeField] private GameObject _infoPanel; // Panel which shows the information about the skill

    [Header("Stats")]
    [SerializeField] private int _pointsUsed = 1; // 1 for each bar filled: MAXIMUM 20 minimum 1
    [SerializeField] private int _pointsMaximum = 20; // Maximum points it takes to fill the bar
    [SerializeField] private float _fillValue = 0.05f; // How many pixels will be filled with each click


    private bool _cooldown = false;


    private void Start()
    {
        _infoPanel.SetActive(false);

        _fillBarBG.fillAmount = _pointsUsed * _fillValue;
        _fillBarFG.fillAmount = _pointsUsed * _fillValue;
        _pointsUsed = 1;
        _fillValue = 1f / _pointsMaximum; // Get 2 decimal place value
    }


    // Function to increase points
    public void PlusButton()
    {
        // Start coroutine when points are less than maximum and cooldown is not active
        if (_pointsUsed < _pointsMaximum && _cooldown == false)
        {
            _pointsUsed++;
            StartCoroutine(IncreaseBar());
            _cooldown = true;
        }
    }


    // Function to decrease points
    public void MinusButton()
    {
        if (_pointsUsed > 1 && _cooldown == false)
        {
            _pointsUsed--;
            StartCoroutine(DecreaseBar());
            _cooldown = true;
        }
    }


    private IEnumerator IncreaseBar()
    {
        // Move the background fill instantly when points change
        _fillBarBG.fillAmount = _pointsUsed * _fillValue;

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


    private IEnumerator DecreaseBar()
    {
        // Move the background fill instantly when points change
        _fillBarFG.fillAmount = _pointsUsed * _fillValue;

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


    public void OnSelect(BaseEventData eventData)
    {
        _infoPanel.SetActive(true);
    }


    public void OnDeselect(BaseEventData eventData)
    {
        _infoPanel.SetActive(false);
    }

}

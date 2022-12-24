using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LevelSystem : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private int _level;
    [SerializeField] private float _currentXp;
    [SerializeField] private float _requiredXp;
    [SerializeField] private int _talentPoint;

    [Header("Timers")]
    [SerializeField] private float _lerpTimer;
    [SerializeField] private float _delayTimer;
    [SerializeField] private float _timer;

    [Header("Bar Objects")]
    [SerializeField] private Image _frontBarFill;
    [SerializeField] private Image _backBarFill;
    [SerializeField] private TMP_Text _txtCurrentLevel;
    [SerializeField] private TMP_Text _txtNextLevel;
    [SerializeField] private TMP_Text _txtXpIndicator;

    [Header("Animations")]
    [SerializeField] private GameObject _levelUpEffect;
    [SerializeField] private GameObject _continueEffect;

    [Header("Multipliers")]
    [SerializeField][Range(1000f, 10000f)] private float _additionMult;
    [SerializeField][Range(2f, 16f)] private float _powerMult;
    [SerializeField][Range(7f, 28f)] private float _divisionMult;
    [SerializeField] private float _timeMult;


    private bool _fullBar = false;


    private void Start()
    {
        // Set Base Stats
        _level = 1;
        _currentXp = 0;
        _talentPoint = 0;


        // Make Animations inactive
        _levelUpEffect.SetActive(false);
        _continueEffect.SetActive(false);


        // Initiate Required Xp
        _requiredXp = CalculateRequiredXp();


        // Start Coroutines
        StartCoroutine(SetStats());
        StartCoroutine(LevelUp());
    }


    private void Update()
    {
        if (PauseMenu.GameIsPaused && PauseMenu._dead)
        {
            // Every frame, update the UI
            UpdateXpUI();

            // Debug test to add exp
            if (Input.GetKeyDown(KeyCode.Equals))
            {
                GainExperienceFlatRate(73);
            }

            // Call levelup function
            if (_frontBarFill.fillAmount == 1)
            {
                _fullBar = true;
            }
            else
            {
                _fullBar = false;
            }
        }
    }


    // Wait until death to start deathxp coroutine
    private IEnumerator SetStats()
    {
        // Set bar fill amount values
        _frontBarFill.fillAmount = _currentXp / _requiredXp;
        _backBarFill.fillAmount = _currentXp / _requiredXp;

        // UI Level Text
        _txtCurrentLevel.text = _level.ToString();
        _txtNextLevel.text = (_level + 1).ToString();


        yield return new WaitUntil(() => PauseMenu._dead);


        // Gain xp from score, as this is on the death screen
        Invoke("GainDeathXP", 2f);
    }


    // Update the UI text and bars each time this function is called
    public void UpdateXpUI()
    {
        // Update text on bar with points relative to fill amount (genius!)
        float tmp = Mathf.RoundToInt(_frontBarFill.fillAmount * _requiredXp);
        float percentage = ((tmp / _requiredXp) * 100.00f);
        _txtXpIndicator.SetText($"<b>EXP:</b>  {tmp} / {_requiredXp}  ({percentage:#0.00}%)");


        // Store decimal fraction of xp
        float xpFraction = _currentXp / _requiredXp;
        // Store the fill amount of bar
        float FXP = _frontBarFill.fillAmount;


        // Make foreground bar lerp after background bar
        if (FXP < xpFraction)
        {
            // Fill the Back bar
            _delayTimer += Time.deltaTime;
            _backBarFill.fillAmount = Mathf.Lerp(_backBarFill.fillAmount, xpFraction, (_delayTimer / (20 * _timeMult))); // The higher the multiplier the slower the lerp


            // Then fill the front bar
            if (_delayTimer >= _timer)
            {
                _lerpTimer += Time.deltaTime;
                float percentComplete = _lerpTimer / (_timeMult * 0.5f); // The higher the multiplier the slower the lerp
                _frontBarFill.fillAmount = Mathf.Lerp(FXP, _backBarFill.fillAmount, percentComplete);
            }
        }


        // Check when bars have stopped moving
        if (_lerpTimer > 3)
        {
            _continueEffect.SetActive(true);
        }
    }


    // Add experience flat rate
    public void GainExperienceFlatRate(float xpGained)
    {
        Debug.Log("Exp Gained");
        // Reset the timers after adding xp
        _currentXp += xpGained;
        _lerpTimer = 0;
        _delayTimer = 0;
    }


    // Add experience scalable
    public void GainExperienceScalable(float xpGained, int passedLevel)
    {
        if (passedLevel < _level)
        {
            float multiplier = 1 + (_level - passedLevel) * 0.1f;
            _currentXp += xpGained * multiplier;
        }
        else
        {
            _currentXp += xpGained;
        }
        _lerpTimer = 0f;
        _delayTimer = 0;
    }


    // Add Score to exp after death screen shows
    private void GainDeathXP()
    {
        if (PauseMenu._dead == true)
        {
            GainExperienceFlatRate(ScoreManager.Instance.Score);
        }
    }


    // Update values and reset bars when level up
    public IEnumerator LevelUp()
    {
        // Reset Timers
        _lerpTimer = 0;
        _delayTimer = 0;

        yield return new WaitForSeconds(1f);

        _levelUpEffect.SetActive(false);

        yield return new WaitUntil(() => _fullBar == true);

        // Experience system
        _level++;
        _frontBarFill.fillAmount = 0f;
        _backBarFill.fillAmount = 0f;
        _currentXp = Mathf.RoundToInt(_currentXp - _requiredXp);

        _requiredXp = CalculateRequiredXp();

        // UI Level Text
        _txtCurrentLevel.text = _level.ToString();
        _txtNextLevel.text = (_level + 1).ToString();

        // Talent Points
        _talentPoint++;

        // Animation
        _levelUpEffect.SetActive(true);


        if (_currentXp >= _requiredXp)
        {
            StartCoroutine(LevelUp());
        }
    }


    // Calcualte required xp for level up algorithm
    private int CalculateRequiredXp()
    {
        int solveForRequiredXp = 0;

        // Loop for everytime we have leveled up
        for (int levelCycle = 1; levelCycle <= _level; levelCycle++)
        {
            // Runescape Algorithm
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + _additionMult * Mathf.Pow(_powerMult, levelCycle / _divisionMult));
        }

        return solveForRequiredXp / 4;
    }
}


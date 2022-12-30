using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class LevelSystem : MonoBehaviour, ISaveable
{
    public static LevelSystem Instance { get; private set; }

    [Header("Level Data")]
    [SerializeField] public int Level = 1;
    [SerializeField] public float CurrentXp;
    [SerializeField] public float RequiredXp;
    [SerializeField] public int TalentPoint;

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
    [SerializeField] private Animator _levelUpEffect;
    [SerializeField] private GameObject _continueEffect;

    [Header("Multipliers")]
    [SerializeField][Range(1000f, 10000f)] private float _additionMult;
    [SerializeField][Range(2f, 16f)] private float _powerMult;
    [SerializeField][Range(7f, 28f)] private float _divisionMult;
    [SerializeField] private float _timeMult;


    private bool _fullBar = false;
    private bool once = true;
    private bool _leveledUp = false;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Set Base Stats
        /*     _level = 1;
            CurrentXp = 0;
            TalentPoint = 0; */


        // Make Animations inactive
        _levelUpEffect.SetTrigger("Normal");
        _leveledUp = false;
        _continueEffect.SetActive(false);


        // Initiate Required Xp
        RequiredXp = CalculateRequiredXp();


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
        _frontBarFill.fillAmount = CurrentXp / RequiredXp;
        _backBarFill.fillAmount = CurrentXp / RequiredXp;

        // UI Level Text
        _txtCurrentLevel.text = Level.ToString();
        _txtNextLevel.text = (Level + 1).ToString();


        yield return new WaitUntil(() => PauseMenu._dead);


        // Gain xp from score, as this is on the death screen
        Invoke("GainDeathXP", 2f);
    }


    // Update the UI text and bars each time this function is called
    public void UpdateXpUI()
    {
        // Update text on bar with points relative to fill amount (genius!)
        float tmp = Mathf.RoundToInt(_frontBarFill.fillAmount * RequiredXp);
        float percentage = ((tmp / RequiredXp) * 100.00f);

        if (!_leveledUp)
        {
            _txtXpIndicator.SetText($"<b>EXP:</b>  {tmp} / {RequiredXp}  ({percentage:#0.00}%)");
        }
        else if (_leveledUp)
        {
            _txtXpIndicator.SetText($"<b>L E V E L  U P !</b>");
        }


        // Store decimal fraction of xp
        float xpFraction = CurrentXp / RequiredXp;
        // Store the fill amount of bar
        float FXP = _frontBarFill.fillAmount;


        // Make foreground bar lerp after background bar
        if (FXP < xpFraction)
        {
            // Fill the Back bar
            _delayTimer += Time.deltaTime;
            _backBarFill.fillAmount = Mathf.Lerp(_backBarFill.fillAmount, xpFraction, (_delayTimer / (15 * _timeMult))); // The higher the multiplier the slower the lerp


            // Then fill the front bar
            if (_delayTimer >= _timer)
            {
                _lerpTimer += Time.deltaTime;
                float percentComplete = _lerpTimer / (_timeMult * 0.5f); // The higher the multiplier the slower the lerp
                _frontBarFill.fillAmount = Mathf.Lerp(FXP, _backBarFill.fillAmount, percentComplete);
            }
        }


        // Check when bars have stopped moving
        if (_lerpTimer > 2 && once)
        {
            _continueEffect.SetActive(true);
            _levelUpEffect.SetTrigger("Normal");
            _leveledUp = false;
            once = false;
        }
    }


    // Add experience flat rate
    public void GainExperienceFlatRate(float xpGained)
    {
        Debug.Log("Exp Gained");
        // Reset the timers after adding xp
        CurrentXp += xpGained;
        _lerpTimer = 0;
        _delayTimer = 0;
    }


    // Add experience scalable
    public void GainExperienceScalable(float xpGained, int passedLevel)
    {
        if (passedLevel < Level)
        {
            float multiplier = 1 + (Level - passedLevel) * 0.1f;
            CurrentXp += xpGained * multiplier;
        }
        else
        {
            CurrentXp += xpGained;
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

        _levelUpEffect.SetTrigger("Normal");
        _leveledUp = false;

        yield return new WaitUntil(() => _fullBar == true);

        _levelUpEffect.SetTrigger("LeveledUp");
        _leveledUp = true;

        // Experience system
        Level++;
        _frontBarFill.fillAmount = 0f;
        _backBarFill.fillAmount = 0f;
        CurrentXp = Mathf.RoundToInt(CurrentXp - RequiredXp);

        RequiredXp = CalculateRequiredXp();

        // UI Level Text
        _txtCurrentLevel.text = Level.ToString();
        _txtNextLevel.text = (Level + 1).ToString();

        // Talent Points
        TalentPoint++;

        if (CurrentXp >= RequiredXp)
        {
            StartCoroutine(LevelUp());
        }
    }


    // Calcualte required xp for level up algorithm
    private int CalculateRequiredXp()
    {
        int solveForRequiredXp = 0;

        // Loop for everytime we have leveled up
        for (int levelCycle = 1; levelCycle <= Level; levelCycle++)
        {
            // Runescape Algorithm
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + _additionMult * Mathf.Pow(_powerMult, levelCycle / _divisionMult));
        }

        return solveForRequiredXp / 4;
    }










    ////////////////////////
    // SAVING AND LOADING //
    public void PopulateSaveData(SaveData a_SaveData)
    {
        a_SaveData.m_PlayerLevel = Level;
        a_SaveData.m_PlayerCurrentXp = CurrentXp;
        a_SaveData.m_PlayerRequiredXp = RequiredXp;
        a_SaveData.m_PlayerTalentPoint = TalentPoint;
    }


    public void LoadFromSaveData(SaveData a_SaveData)
    {
        Level = a_SaveData.m_PlayerLevel;
        CurrentXp = a_SaveData.m_PlayerCurrentXp;
        RequiredXp = a_SaveData.m_PlayerRequiredXp;
        TalentPoint = a_SaveData.m_PlayerTalentPoint;
    }
    // SAVING AND LOADING //
    ////////////////////////
}


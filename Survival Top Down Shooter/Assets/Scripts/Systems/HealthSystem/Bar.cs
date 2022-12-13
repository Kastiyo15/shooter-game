using System.Collections;
using System.Collections.Generic;
using TMPro;
using Scripts.Systems.HealthSystem;
using UnityEngine;


public class Bar : MonoBehaviour
{
    /*     [field: SerializeField]
        public int MaxValue { get; private set; }
        [field: SerializeField]
        public int Value { get; private set; } */

    [Header("Base Values")]
    [SerializeField]
    private int MaxValue;
    [SerializeField]
    private int Value;

    // Health stats
    [Header("Health")]
    [SerializeField]
    private Health _health;
    [SerializeField]
    private TMP_Text _hpIndicator;

    // Score Stats
    [Header("Score")]
    [SerializeField]
    private ScoreManager _score;
    [SerializeField]
    private TMP_Text _scoreMult;
    private int mult;

    [Header("Bars")]
    [SerializeField]
    private RectTransform _topBar;
    [SerializeField]
    private RectTransform _bottomBar;

    [SerializeField]
    private float _animationSpeed = 10f;

    private float _fullWidth;

    private float TargetWidth => Value * _fullWidth / MaxValue;

    private Coroutine _adjustBarWidthCoroutine;


    private void Start()
    {
        _fullWidth = _topBar.rect.width;
        SetInitialValues();
    }


    private void SetInitialValues()
    {
        if (CompareTag("HealthBar"))
        {
            // Set the initial text of the hpindicator
            _hpIndicator.SetText($"{_health.Hp}/{_health.MaxHp}");

            // Set Values to Health
            MaxValue = _health.MaxHp;
            Value = MaxValue;
        }
        else if (CompareTag("ScoreBar"))
        {
            mult = ScoreManager.Instance.ScoreMultiplier;

            _scoreMult.SetText($"x{mult}");

            // Set Values to Score
            MaxValue = _score.ScorePerKill * (20 * mult);
            Value = 0;
            Change(Value);
        }
    }


    public void Change(HealthChangeArgs args)
    {
        if (CompareTag("HealthBar"))
        {
            Change(args.ActualChange);
        }
    }


    private IEnumerator AdjustBarWidth(int amount)
    {
        var suddenChangeBar = amount >= 0 ? _bottomBar : _topBar;
        var slowChangeBar = amount >= 0 ? _topBar : _bottomBar;

        if (suddenChangeBar.rect.width != TargetWidth)
        {
            suddenChangeBar.SetWidth(TargetWidth);
            while (Mathf.Abs(suddenChangeBar.rect.width - slowChangeBar.rect.width) > 1f)
            {
                slowChangeBar.SetWidth(Mathf.Lerp(slowChangeBar.rect.width, TargetWidth, Time.deltaTime * _animationSpeed));
                // Wait until next frame
                yield return null;
            }
            slowChangeBar.SetWidth(TargetWidth);
        }
        else if (suddenChangeBar.rect.width == TargetWidth)
        {
            ScoreManager.Instance.IncreaseScoreMultiplier();
            SetInitialValues();
        }
    }


    public void Change(int amount)
    {
        Value = Mathf.Clamp(Value + amount, 0, MaxValue);
        if (_adjustBarWidthCoroutine != null)
        {
            StopCoroutine(_adjustBarWidthCoroutine);
        }

        _adjustBarWidthCoroutine = StartCoroutine(AdjustBarWidth(amount));

        if (CompareTag("HealthBar"))
        {
            // Set Health Text
            _hpIndicator.SetText($"{Value}/{_health.MaxHp}");
        }

        if (CompareTag("ScoreBar"))
        {
            _scoreMult.SetText($"x{mult}");
        }
    }



}


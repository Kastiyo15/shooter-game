using System.Collections;
using UnityEngine;
using TMPro;
using Scripts.Systems.MessageSystem;


[System.Serializable]
public class ScoreManager : MonoBehaviour, ISaveable
{
    public static ScoreManager Instance { get; private set; }


    public long Score { get; private set; }
    public int ScorePerKill = 5; // base value, each enemy has its own score
    public int ScorePerSecond = 10;
    public float SPSTimer = 1;
    public int ScoreMultiplier = 1;

    public int BulletsFiredScore = 0;
    public int TotalDamageScore = 0;
    public int TotalAliveScore = 0;
    public int TotalKillScore = 0;


    [Header("Floating Text Pos")]
    [SerializeField] private float _yPosition;
    [SerializeField] private float _xPosition;


    // Score References
    [Header("Score References")]
    [SerializeField] private TMP_Text _scoreText; // Score at the top of the screen
    [SerializeField] private GameObject _bar;
    [SerializeField] private GameObject _messagePrefab; // Score text after each kill
    private Bar _scoreBar;


    // Player references
    [Header("Player References")]
    [SerializeField] private Animator _crosshair;
    [SerializeField] private GameObject _player;
    private GameManager _playerController;


    // Alive Score Coroutine
    private IEnumerator coroutine;


    //  Run this before anything else
    private void Awake()
    {
        Instance = this;
        _playerController = _player.GetComponent<GameManager>();
        _scoreBar = _bar.GetComponent<Bar>();
    }


    // Start coroutine to add score every second
    void Start()
    {
        coroutine = AliveScore(ScorePerSecond);
        StartCoroutine(coroutine);
    }


    // Increase score by input amount then update score ui
    public void IncreaseScore(int amount)
    {
        int calcValue = amount * ScoreMultiplier;
        Score += calcValue;

        UpdateScoreDisplay();

        AddToTotals(calcValue);

        FloatingScoreText(calcValue);
    }


    // Function to add 1 to Score Multiplier variable
    public void IncreaseScoreMultiplier()
    {
        if (ScoreMultiplier < 4)
        {
            ScoreMultiplier++;
        }
    }


    // Update the score ui
    public void UpdateScoreDisplay()
    {
        _scoreText.text = string.Format("{0}", Score);

        /*         if (Score < 1000)
                {
                    //_scoreText.text = string.Format("<b>Score:</b> {0:0}", Score);
                    _scoreText.text = string.Format("{0:0}", Score);
                }
                else if (Score >= 1000 && Score < 1000000)
                {
                    //_scoreText.text = string.Format("Score: {0:0,000}", Score);
                    _scoreText.text = string.Format("{0:0,000}", Score);
                }
                else if (Score >= 1000000 && Score < 1000000000)
                {
                    //_scoreText.text = string.Format("Score: {0:0,000,000}", Score);
                    _scoreText.text = string.Format("{0:0,000,000}", Score);
                } */
    }


    // Create floating text underneath the score
    private void FloatingScoreText(float value)
    {
        Vector3 newPosition = new Vector3(transform.position.x + _xPosition, transform.position.y + _yPosition, transform.position.z);
        var txt = Instantiate(_messagePrefab, newPosition, Quaternion.identity);
        txt.GetComponentInChildren<TMP_Text>().text = "+" + value.ToString();
    }


    // Increase Score Bar Values
    public void KillScore(int amount)
    {
        _crosshair.Play("CrosshairKillFlash", -1, 0f);
        _scoreBar.Change(amount);
    }


    public void AddToTotals(int amount)
    {
        // Work out Score Per Second Score
        if ((amount / ScoreMultiplier) == ScorePerSecond)
        {
            TotalAliveScore += amount;
        }
    }


    // Add to score every 1 second
    private IEnumerator AliveScore(int sps)
    {
        while (true && _player.activeInHierarchy)
        {
            if (ScorePerSecond > sps)
            {
                sps = ScorePerSecond;
            }
            else
            {
                yield return new WaitForSeconds(SPSTimer);
                IncreaseScore(sps);
            }
        }
    }


    // Add damage and bullet scores to Score
    public void DeathScore()
    {
        Score += BulletsFiredScore + TotalDamageScore;
    }


    ////////////////////////
    // SAVING AND LOADING //
    public void PopulateSaveData(SaveData a_SaveData)
    {
        a_SaveData.m_ScorePerKill = ScorePerKill; // base value, each enemy has its own score
        a_SaveData.m_ScorePerSecond = ScorePerSecond;
        a_SaveData.m_SPSTimer = SPSTimer;
    }


    public void LoadFromSaveData(SaveData a_SaveData)
    {
        ScorePerKill = a_SaveData.m_ScorePerKill;
        ScorePerSecond = a_SaveData.m_ScorePerSecond;
        SPSTimer = a_SaveData.m_SPSTimer;
    }
    // SAVING AND LOADING //
    ////////////////////////
}

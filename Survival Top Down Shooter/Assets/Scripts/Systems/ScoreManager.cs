using System.Collections;
using UnityEngine;
using TMPro;
using Scripts.Systems.MessageSystem;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }


    public long Score { get; private set; }
    public int ScorePerKill = 10; // base value, each enemy has its own score
    public int ScorePerSecond = 1;
    public float SPSTimer = 1;
    public int ScoreMultiplier = 1;


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
        _scoreBar.Change(amount);
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
}
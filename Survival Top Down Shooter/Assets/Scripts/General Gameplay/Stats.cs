using UnityEngine;


public class Stats : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraShake _cameraShake;
    [SerializeField] private Enemy _enemyScore;
    [SerializeField] private GameObject _effectPrefab; // For Death animation


    // Used to calculate SPK * Enemy value
    public int _killValue;


    // Run before anything else
    void Awake()
    {
        Camera _cam = Camera.main;
        _cameraShake = _cam.GetComponent<CameraShake>();
    }


    void Start()
    {
        if (CompareTag("Enemy"))
        {
            _enemyScore.GetComponent<Enemy>();
            _killValue = _enemyScore._scoreValue * (ScoreManager.Instance.ScorePerKill);
        }
    }


    // Death function
    public void Die()
    {
        if (CompareTag("Player"))
        {
            _cameraShake._start = true;
        }

        if (CompareTag("Enemy"))
        {
            GameObject deathEffect = Instantiate(_effectPrefab, transform.position, Quaternion.identity);

            // Increase Kill Count
            KillCounter.FindObjectOfType<KillCounter>().UpdateKillCounter();

            // Increase player score
            ScoreManager.Instance.IncreaseScore(_killValue);
            ScoreManager.Instance.TotalKillScore += _killValue * ScoreManager.Instance.ScoreMultiplier;

            // Increase Score bar
            ScoreManager.Instance.KillScore(_killValue);

            Destroy(gameObject);
        }
    }
}
